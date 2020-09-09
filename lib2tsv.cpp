
#include <iostream>
#include <fstream>
#include <string> 
#include <vector>
#include <map>
#include <zlib.h>
#include <sqlite3.h>

#include <OpenMS/ANALYSIS/OPENSWATH/TransitionTSVFile.h>
#include <OpenMS/ANALYSIS/TARGETED/TargetedExperiment.h>

#include <OpenMS/CHEMISTRY/ModificationsDB.h>

#include <OpenMS/APPLICATIONS/TOPPBase.h>
#include <OpenMS/CONCEPT/Exception.h>
#include <OpenMS/CONCEPT/ProgressLogger.h>
#include <OpenMS/CONCEPT/LogStream.h>

using namespace OpenMS;

class PSM {
public:
	int id;
	int decoy;
	std::string peptideSeq;
	double precursorMZ;
	int precursorCharge;
	std::string peptideModSeq;
	int numPeaks;
	double ionMobility;
	double retentionTime;
	std::vector<double> peakMZ;
	std::vector<float> peakIntensity;
};


class Lib2Tsv
	: public TOPPBase
{
public:
	Lib2Tsv() :TOPPBase("Lib2Tsv", "Convert blib/sptxt format library to tsv format library", false)
	{
	}

protected:

	typedef  TargetedExperiment::Protein Protein;
	typedef  TargetedExperiment::Peptide Peptide;
	typedef  TargetedExperiment::Transition Transition;
	typedef  TargetedExperiment::RetentionTime RetentionTime;
	typedef  TargetedExperiment::Peptide::Modification Modification;

	Param getSubsectionDefaults_(const  String& name) const override
	{
		Param p;
		return p;
	}
	void registerOptionsAndFlags_() override
	{
		registerInputFile_("db", "<db.fasta>", "", "non-decoy fasta format database");
		setValidFormats_("db", ListUtils::create< String>("fasta"), false);

		registerInputFile_("in", "<in.blib/in.sptxt>", "", "non-decoy blib/sptxt format library");
		setValidFormats_("in", ListUtils::create< String>("blib,sptxt"), false);

		registerInputFile_("unimod_file", "<unimod.xml>", "", "Subset of Unimod XML file (http://www.unimod.org/xml/unimod.xml) describing residue modifiability", false);
		setValidFormats_("unimod_file", ListUtils::create< String>("xml"), false);

		registerOutputFile_("out", "<out.tsv>", "", "output non-decoy tsv format library");
		setValidFormats_("out", ListUtils::create< String>("tsv"), false);
	}

	sqlite3* blib_db = nullptr;

	std::vector<std::pair<std::string, std::string>> fasta_db;

	std::map<int, PSM*> data;

	TargetedExperiment exp;

	ExitCodes main_(int, const char**) override
	{
		String unimod_file = getStringOption_("unimod_file");
		if (!unimod_file.empty()) {
			if (!ModificationsDB::isInstantiated()) // We need to ensure that ModificationsDB was not instantiated before!
			{
				ModificationsDB* ptr = ModificationsDB::initializeModificationsDB(unimod_file, String(""), String(""));
				OPENMS_LOG_INFO << "Unimod XML: " << ptr->getNumberOfModifications() << " modification types and residue specificities imported from file: " << unimod_file << std::endl;
			}
			else
			{
				throw Exception::Precondition(__FILE__, __LINE__, OPENMS_PRETTY_FUNCTION, "ModificationsDB has been instantiated before and can not be generated from the provided Unimod XML file.");
			}
		}
		load_fasta(getStringOption_("db"));

		if (getStringOption_("in").hasSuffix("blib")) {
			open_blib(getStringOption_("in"));

			load_blib();

			close_blib();
		}
		else {
			read_sptxt(getStringOption_("in"));
		}

		String ofn = getStringOption_("out");
		TransitionTSVFile tsv;
		tsv.convertTargetedExperimentToTSV(ofn.c_str(), exp);

		return EXECUTION_OK;
	}

	void analysisMod(PSM &psm, Peptide& peptide,bool fixMz=true) {
		std::vector<Modification> mods;
		AASequence aa_sequence;
		String sequence = "";
		bool mflag = false;
		for (int i = 0; i < psm.peptideModSeq.size();++i) {
			sequence.push_back(psm.peptideModSeq[i]);
			switch (psm.peptideModSeq[i]) {
			case '(':
			case '[':
			case ')':
			case ']':
				mflag = !mflag;
				continue;
			case 'B':
			case 'J':
			case 'O':
			case 'U':
			case 'X':
			case 'Z':
				if (!mflag) {
					sequence.pop_back();
				}
			}
		}
		try
		{
			aa_sequence = AASequence::fromString(sequence);
			//aa_sequence.getMonoWeight()
		}
		catch (Exception::InvalidValue& e)
		{
			OPENMS_LOG_DEBUG << "Invalid sequence when parsing '" << sequence << "'" << std::endl;
			std::cerr << "Error while reading file (use 'force_invalid_mods' parameter to override): " << e.what() << std::endl;
			throw Exception::IllegalArgument(__FILE__, __LINE__, OPENMS_PRETTY_FUNCTION,
				"Invalid input, cannot parse: " + sequence);

		}
		peptide.sequence = aa_sequence.toUnmodifiedString();	 
		psm.peptideModSeq = aa_sequence.toString();
		if (fixMz) {
			psm.precursorMZ = aa_sequence.getMonoWeight(OpenMS::Residue::Full, psm.precursorCharge) / psm.precursorCharge;
		}


		if (aa_sequence.hasNTerminalModification())
		{
			const ResidueModification& rmod = *(aa_sequence.getNTerminalModification());
			addModification_(mods, -1, rmod);
		}
		if (aa_sequence.hasCTerminalModification())
		{
			const ResidueModification& rmod = *(aa_sequence.getCTerminalModification());
			addModification_(mods, aa_sequence.size(), rmod);
		}
		for (Size i = 0; i != aa_sequence.size(); i++)
		{
			if (aa_sequence[i].isModified())
			{
				const ResidueModification& rmod = *(aa_sequence.getResidue(i).getModification());
				addModification_(mods, i, rmod);
			}
		}
		peptide.mods.assign(mods.begin(), mods.end());
	}

	void addModification_(std::vector<Modification>& mods,
		int location,
		const ResidueModification& rmod)
	{
		Modification mod;
		mod.location = location;
		mod.mono_mass_delta = rmod.getDiffMonoMass();
		mod.avg_mass_delta = rmod.getDiffAverageMass();
		mod.unimod_id = rmod.getUniModRecordId(); // NOTE: will be -1 if not found in UniMod (e.g. user-defined modifications)
		mods.push_back(mod);
	}

	void load_fasta(const std::string& path) {
		std::ifstream fin(path.c_str());

		if (!fin) {
			fin.close();
			throw - 1;
		}

		std::string line, pid;

		while (std::getline(fin, line)) {
			if (line.find('>') == 0) {
				auto p = line.find(' ');
				if (p == line.npos) {
					pid = line.substr(1);
				}
				else {
					pid = line.substr(1, p - 1);
				}
				fasta_db.push_back(std::make_pair(pid, ""));
			}
			else {
				for (int i = 0; i < line.size(); ++i) {
				}
				for (int i = 0; i < line.size(); ++i) {
					char ch = line[i];
					switch (ch) {
					case 'I':
						fasta_db.back().second.push_back('L');						
					case 'B':
					case 'J':
					case 'O':
					case 'U':
					case 'X':
					case 'Z':
						continue;
					}
					fasta_db.back().second.push_back(ch);
				}
				fasta_db.back().second += line;
			}
		}

		fin.close();

		StringList subs;

		for (auto& p : fasta_db) {
			Protein pro;
			pro.id = p.first;

			subs.clear();
			pro.id.split('|', subs);
			if (subs.size() == 3) {
				subs.pop_back();

				std::vector< CVTerm> t;
				t.push_back(CVTerm("MS:1000885", "", "", subs.back()));
				pro.setCVTerms(t);
			}

			exp.addProtein(pro);
		}

		std::cout << "loaded fasta" << std::endl;
	}


	void load_blib() {

		std::string sql = "SELECT id,peptideSeq,precursorMZ,precursorCharge,peptideModSeq,numPeaks,ionMobility,retentionTime FROM RefSpectra;";
		sqlite3_stmt* stmt = nullptr;


		int cd = sqlite3_prepare_v2(blib_db, sql.c_str(), -1, &stmt, nullptr);
		check(cd);


		while (sqlite3_step(stmt) == SQLITE_ROW) {

			const int id = sqlite3_column_int(stmt, 0);
			const char* peptideSeq = (const char*)sqlite3_column_text(stmt, 1);
			const double precursorMZ = sqlite3_column_double(stmt, 2);
			const int precursorCharge = sqlite3_column_int(stmt, 3);
			const char* peptideModSeq = (const char*)sqlite3_column_text(stmt, 4);
			const int numPeaks = sqlite3_column_int(stmt, 5);
			const double ionMobility = sqlite3_column_double(stmt, 6);
			const double retentionTime = sqlite3_column_double(stmt, 7);
			PSM* ptr = new PSM();
			data[id] = ptr;
			PSM& o = *ptr;
			o.decoy = 0;
			o.id = id;
			o.ionMobility = ionMobility;
			o.numPeaks = numPeaks;
			o.peptideModSeq = peptideModSeq;
			o.peptideSeq = peptideSeq;
			o.precursorCharge = precursorCharge;
			o.precursorMZ = precursorMZ;
			o.retentionTime = retentionTime;
		}

		cd = sqlite3_finalize(stmt);
		check(cd);

		std::vector<PSM*> ptrs;
		for (auto j = data.begin(); j != data.end(); ++j) {
			ptrs.push_back(j->second);
		}


		std::cout << "loaded precursor" << std::endl;

		sql = "SELECT RefSpectraID,peakMZ,peakIntensity FROM RefSpectraPeaks;";
		cd = sqlite3_prepare_v2(blib_db, sql.c_str(), -1, &stmt, nullptr);
		check(cd);

		while (sqlite3_step(stmt) == SQLITE_ROW) {

			const int id = sqlite3_column_int(stmt, 0);
			PSM& p = *data[id];


			int n = sqlite3_column_bytes(stmt, 1);
			const void* mz_src = sqlite3_column_blob(stmt, 1);

			if (n == p.numPeaks * sizeof(double)) {
				p.peakMZ = std::vector<double>((const double*)mz_src, (const double*)(mz_src)+p.numPeaks);
			}
			else {

				double* mz = new double[p.numPeaks];
				uLongf t = p.numPeaks * sizeof(double);

				int cd = uncompress((Bytef*)mz, &t, (const Bytef*)mz_src, n);
				check(cd);
				p.peakMZ = std::vector<double>(mz, mz + p.numPeaks);

				delete[] mz;
			}

			n = sqlite3_column_bytes(stmt, 2);
			const void* intn_src = sqlite3_column_blob(stmt, 2);

			if (n == p.numPeaks * sizeof(float)) {
				p.peakIntensity = std::vector<float>((const float*)intn_src, (const float*)(intn_src)+p.numPeaks);
			}
			else {

				float* intn = new float[p.numPeaks];
				uLongf t = p.numPeaks * sizeof(float);
				int cd = uncompress((Bytef*)intn, &t, (const Bytef*)intn_src, n);
				check(cd);
				p.peakIntensity = std::vector<float>(intn, intn + p.numPeaks);

				delete[] intn;
			}
		}
		cd = sqlite3_finalize(stmt);
		check(cd);

		std::cout << "loaded lib" << std::endl;

		for (int i = 0; i < ptrs.size(); ++i) {
			PSM& pp = *ptrs[i];

			RetentionTime rt;
			rt.setRT(pp.retentionTime);

			Peptide pep;
			analysisMod(pp, pep);
			String label = std::to_string(i + 1) + "_" + pp.peptideModSeq + "_" + std::to_string(pp.precursorCharge);
			pep.id = label;
			pep.setPeptideGroupLabel(label);
			pep.setChargeState(pp.precursorCharge);
			pep.setDriftTime(pp.ionMobility);
			pep.rts.push_back(rt);

			String s = pep.sequence;
			for (int j = 0; j < s.size(); ++j) {
				if (s[j] == 'I') {
					s[j] = 'L';
				}
			}
			std::vector<int> ids;
#pragma omp parallel for
			for (int i = 0; i < fasta_db.size(); ++i) {
				const char* pro = fasta_db[i].second.c_str();
				if (strstr(pro, s.c_str()) != nullptr) {
#pragma omp critical
					ids.push_back(i);
				}
			}			
			if (ids.empty()) {
				continue;
			}
			for (int i:ids){
				pep.protein_refs.push_back(fasta_db[i].first);
			}
			std::sort(pep.protein_refs.begin(), pep.protein_refs.end());

			//pep.sequence = pp.peptideSeq;
			exp.addPeptide(pep);


			for (int j = 0; j < pp.numPeaks; ++j) {
				Transition tran; 
				tran.setName(std::to_string(j)+"_"+label);
				tran.setRetentionTime(rt);
				if (pp.decoy) {
					tran.setDecoyTransitionType(Transition::DECOY);
				}
				else {
					tran.setDecoyTransitionType(Transition::TARGET);
				}
				tran.setLibraryIntensity(pp.peakIntensity[j]);
				tran.setPrecursorMZ(pp.precursorMZ);
				tran.setProductMZ(pp.peakMZ[j]);
				tran.setPeptideRef(pep.id);
				exp.addTransition(tran);
			}
			delete ptrs[i];
		}
		std::cout << "mapped protein name" << std::endl;


	}

	void check(int cd) {
		if (cd == SQLITE_OK) {
			return;
		}
		throw cd;
	}

	void open_blib(const std::string& path) {
		sqlite3_initialize();
		int cd = sqlite3_open(path.c_str(), &blib_db);
		check(cd);
	}

	void close_blib() {
		if (blib_db == nullptr) {
			return;
		}
		while (1) {
			try {
				int cd = sqlite3_close(blib_db);
				check(cd);
				//delete blib_db;
				sqlite3_shutdown();
				return;
			}
			catch (int ex) {
				if (ex == SQLITE_BUSY) {
					sqlite3_stmt* t = sqlite3_next_stmt(blib_db, nullptr);
					if (t != nullptr) {
						int cd2 = sqlite3_finalize(t);
						check(cd2);
					}
				}
				else {
					throw ex;
				}
			}
		}
	}


	void read_sptxt(const std::string& path) {
		std::ifstream fin(path);
		OpenMS::String line;
		OpenMS::String txt;
		bool begin = false;
		int n = 0;
		int peaks;
		int id;
		int chg;
		int pos;
		double pmz;
		double rt;
		std::string seq;

		PSM* ptr = nullptr;
		while (std::getline(fin, line)) {
			line.trim();
			if (line.empty()) {
				continue;
			}
			if (n > 0) {
				OpenMS::StringList items;
				line.split("\t", items);
				ptr->peakMZ.push_back(items[0].toDouble());
				ptr->peakIntensity.push_back(items[1].toDouble());
				--n;
				if (n == 0) {
					begin = false;
					ptr->decoy = 0;
					ptr->id = id;
					ptr->ionMobility = -1;
					ptr->numPeaks = peaks;
					ptr->peptideSeq = seq;
					ptr->peptideModSeq = seq;
					ptr->precursorCharge = chg;
					ptr->precursorMZ = pmz;
					ptr->retentionTime = rt;
					data[id] = ptr;
				}
				continue;
			}
			if (begin) {
				txt = line.suffix(' ');
				switch (line[0]) {
				case 'L':
					//LibID
					id = txt.toInt();
					break;
				case 'P':
					//PrecursorMZ
					pmz = txt.toDouble();
					break;
				case 'C':
					//Comment
					pos = line.find("RetentionTime=");
					if (pos != line.npos) {
						txt = line.substr(pos + 14);
						pos = txt.find(',');
						rt = txt.substr(0, pos).toDouble();
						continue;
					}
					pos = line.find("iRT=");
					if (pos != line.npos) {
						txt = line.substr(pos + 4);
						pos = txt.find(',');
						rt = txt.substr(0, pos).toDouble();
						continue;
					}
					break;
				case 'N':
					//NumPeaks
					n = txt.toInt();
					peaks = n;
					break;
				default:
					break;
				}
				continue;
			}
			if (line.hasPrefix("N")) {//Name:
				begin = true;
				ptr = new PSM();
				n = -1;
				rt = 0;
				txt = line.suffix(' ');
				chg = txt.back() - '0';
				txt.pop_back();
				txt.pop_back();
				seq = txt;
				continue;
			}
		}

		fin.close();

		std::vector<PSM*> ptrs;
		for (auto j = data.begin(); j != data.end(); ++j) {
			ptrs.push_back(j->second);
		}

		std::cout << "loaded lib" << std::endl;

		for (int i = 0; i < ptrs.size(); ++i) {
			PSM& pp = *ptrs[i];

			RetentionTime rt;
			rt.setRT(pp.retentionTime);

			Peptide pep;
			analysisMod(pp, pep);
			String label = std::to_string(i + 1) + "_" + pp.peptideModSeq + "_" + std::to_string(pp.precursorCharge);
			pep.id = label;
			pep.setPeptideGroupLabel(label);
			pep.setChargeState(pp.precursorCharge);

			String seq = pep.sequence.c_str();
			for (int i = 0; i < seq.size(); ++i) {
				if (seq[i] == 'I') {
					seq[i] = 'L';
				}
			}
			std::vector<int> ids;
#pragma omp parallel for
			for (int i = 0; i < fasta_db.size(); ++i) {
				const char* pro = fasta_db[i].second.c_str();
				if (strstr(pro, seq.c_str()) != nullptr) { 
#pragma omp critical
					ids.push_back(i);
				}
			}
			if (ids.empty()) {
				continue;
			}
			for (int i:ids){
				pep.protein_refs.push_back(fasta_db[i].first);				
			}
			std::sort(pep.protein_refs.begin(), pep.protein_refs.end());

			pep.rts.push_back(rt);
			exp.addPeptide(pep);

			for (int j = 0; j < pp.numPeaks; ++j) {
				Transition tran;

				tran.setName(std::to_string(j) + "_" + label);
				tran.setRetentionTime(rt);
				if (pp.decoy) {
					tran.setDecoyTransitionType(Transition::DECOY);
				}
				else {
					tran.setDecoyTransitionType(Transition::TARGET);
				}
				tran.setLibraryIntensity(pp.peakIntensity[j]);
				tran.setPrecursorMZ(pp.precursorMZ);
				tran.setProductMZ(pp.peakMZ[j]);
				tran.setPeptideRef(pep.id);
				exp.addTransition(tran);
			}
			delete ptrs[i];
		}
		std::cout << "mapped protein name" << std::endl;

	}




};



int main(int argc, const char** argv)
{
	Lib2Tsv tool;
	return tool.main(argc, argv);
}
