import java.io.File;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map.Entry;
import java.util.Scanner;
import java.util.TreeMap;
import java.util.TreeSet;

public class UpdateReport {

	
	private static String toPepFilename(String name) {
		String fn = name;

		String[] fnt = fn.split("[/;]");
		

		ArrayList<String> tmp=new ArrayList<>();
		
		for (String s:fnt) {
			if (s.replaceAll("[0-9]","").isEmpty()) {
				continue;
			}
			tmp.add(s);
		}
		String t3 = null;
		if (tmp.size() > 3) {
			t3 = tmp.size() + "/" + tmp.get(0) + "/" + tmp.get(1) + "/" + tmp.get(2);
		} else {
			t3=""+tmp.size();
			for (int i=0;i<tmp.size();++i) {
				t3=t3+"/"+tmp.get(i);
			}
		}

		fn = t3.replaceAll("[\\\\/:*?\"<>|]", "_");
		if (fn.length() > 240) {
			fn = (fnt[0] + "/" + fnt[1]).replaceAll("[\\\\/:*?\"<>|]",
					"_");
		}

		return fn;
	}

	static int filename_pos=3;
	static int ProteinName_pos=26;
	static int decoy_pos=1;	
	static int Charge_pos=13;
	static int Intensity_pos=15;
	

	public static void main(String[] args) throws FileNotFoundException{
		
		System.out.println("Usage : java UpdateReport aligned.tsv");
		
		if (args.length!=1) {
			return ;
		}
		
		// ../draw/ aligned.tsv
		String fn=args[0];
		
		Scanner s = new Scanner(new File(fn));

		String head=s.nextLine();
		scanHeader(head);
		//{filename}
		TreeSet<String> fns = new TreeSet<>();		
		
		//{ProteinName} -> "{filename}	{FullPeptideName}/{Charge}	{Intensity}	{ProteinNametoPepFilename}"	
		TreeMap<String, TreeSet<String>> prot = new TreeMap<>();

		while (s.hasNextLine()) {
			String[] line = s.nextLine().split("\t");
			fns.add(line[filename_pos]);
			if (line[decoy_pos].equals("1")){
				continue;
			}
			if (!prot.containsKey(line[ProteinName_pos])) {
				prot.put(line[ProteinName_pos], new TreeSet<>());
			}
			TreeSet<String> st = prot.get(line[ProteinName_pos]);

			//FullPeptideNameWithUniModText_pos == line.length-1
			st.add(new File(line[filename_pos]).getName() + "\t" + line[line.length-1] + "/" + line[Charge_pos] + "\t" + line[Intensity_pos]+ "\t" + toPepFilename(line[ProteinName_pos]));
			
		}
		s.close();

		PrintWriter rppw = new PrintWriter(fn + "_quant.pep.csv");		
		
		do
		{

			Scanner peps=new Scanner(new File(fn+"_pep.csv"));
			
			rppw.print(peps.nextLine());
			for (String f:fns) {
				rppw.print(",");
				rppw.print(new File(f).getName());
			}
			rppw.print(",");
			for (String f:fns) {
				rppw.print(",");
				rppw.print(new File(f).getName());
			}
			rppw.println();

			HashMap<String,Integer> fnidx=new HashMap<>();
			ArrayList<String> fnArr=new ArrayList<>();
			for (String f:fns) {
				fnArr.add(new File(f).getName());
			}

			for (String f:fns) {
				int T=fnidx.size();
				fnidx.put(new File(f).getName(),T);				
			}
			
			//{pepWithModName}/{Charge} -> [{Intensity}]
			TreeMap<String,double[]> omap=new TreeMap<>();
			

			//{ProteinName} -> "{filename}	{FullPeptideName}/{Charge}	{Intensity}	{ProteinNametoPepFilename}"	
			for (Entry<String, TreeSet<String>> pe:prot.entrySet()) {
				TreeSet<String> pepSet = pe.getValue();
				for (String sp:pepSet) {
					String[] info=sp.split("\t");
					int idx=fnidx.get(info[0]);
					double intn=Double.valueOf(info[2]);
															
					if (!omap.containsKey(info[1])) {
						
						omap.put(info[1], new double[fns.size()]);
					}
					omap.get(info[1])[idx]+=intn;					
				}
			}
						
			double[] sumintn=new double[fns.size()];
			
			for (Entry<String, double[]> oe:omap.entrySet()) {
				double[] v=oe.getValue();
				for (int i=0;i<v.length;++i) {
					sumintn[i]+=v[i];
				}
			}
			
			double[] ratio=sumintn.clone();
			for (int i=0;i<ratio.length;++i) {
				ratio[i]=sumintn[0]/sumintn[i];
			}
			
			int pepWithModName=3;
			int Charge=5;
			
			while (peps.hasNextLine()) {
				String line=peps.nextLine();
				
				rppw.print(line);
				
				String[] items=line.split(",");
				
				double[] arr = omap.get(items[pepWithModName]+"/"+items[Charge]);
				
				for (int i=0;i<arr.length;++i) {
					rppw.print(",");
					rppw.print(arr[i]);
				}
				rppw.print(",");
				for (int i=0;i<arr.length;++i) {
					rppw.print(",");
					rppw.print(arr[i]*ratio[i]);
				}
				rppw.println();
			}
			
			peps.close();
			
		}while (false);
		
		
		rppw.close();
				
		rppw = new PrintWriter(fn + "_quant.prot.csv");

		StringBuffer out = new StringBuffer();		

		//{index} -> {filename}
		TreeMap<Integer, String> fnmap = new TreeMap<>();
		
		rppw.print("ProteinName,UniquePeptideCount");
		int n = 0;
		for (String f : fns) {
			rppw.print(",");
			rppw.print(new File(f).getName());			
			fnmap.put(n, new File(f).getName());
			++n;
		}
		rppw.print(",");
		for (String f : fns) {
			rppw.print(",");
			rppw.print(new File(f).getName());	
		}
		rppw.println();
		
//		double min=-1;
		
		TreeSet<String> uniq=new TreeSet<>();

		//{ProteinName} -> "{filename}	{FullPeptideName}/{Charge}	{Intensity}	{ProteinNametoPepFilename}"	
		//TreeMap<String, TreeSet<String>> prot = new TreeMap<>();
		for (Entry<String, TreeSet<String>> e : prot.entrySet()){
			TreeSet<String> set = new TreeSet<>();
			for (String l : e.getValue()) {
				set.add(l.split("\t")[1]);
			}
			if (set.isEmpty()) {
				continue;			
			}
			
			String[] pros = e.getKey().split("[/;]");
			int npros=0;
			for (String p:pros) {
				if (!p.replaceAll("[0-9]", "").isEmpty()) {
					++npros;
				}
			}
			if (npros==1){
				for (String p:pros) {
					if (!p.replaceAll("[0-9]", "").isEmpty()) {
						uniq.add(p);
					}
				}
			}
			
			out.append(e.getKey() + "," + set.size());
			for (Entry<Integer, String> et : fnmap.entrySet()) {
				out.append(",");
				ArrayList<Double> arr = new ArrayList<>();

				for (String l : e.getValue()) {
					String[] items = l.split("\t");
					if (et.getValue().equals(items[0])) {
						arr.add(Double.parseDouble(items[2]));
					}
				}
				double sum = 0;
				for (double d : arr) {
					sum += d;
				}
				if (arr.size() > 3) {

					arr.sort((a, b) -> {
						return Double.compare(b, a);
					});
					for (int i = 3; i < arr.size(); ++i) {
						sum -= arr.get(i);
					}
				}
				out.append(sum);
			}

			out.append(System.lineSeparator());
		}
		
		TreeMap<String,double[]> omap=new TreeMap<>();
		
		s=new Scanner(out.toString());
		while (s.hasNextLine()){
			String[] items = s.nextLine().split(",");

			boolean flag=false;
			String[] pros = items[0].split("[/;]");
			int npros=0;
			int pi=-1;
			for (int i=0;i<pros.length;++i) {
				if (!pros[i].replaceAll("[0-9]", "").isEmpty()) {
					++npros;
					if (uniq.contains(pros[i])) {
						flag=true;
					}
					if (pi<0) {
						pi=i;
					}
				}
			}
			if (npros>1&&flag){
				continue;
			}
			String p=pros[pi];
			if (!omap.containsKey(p)){
				omap.put(p, new double[1+fns.size()]);
			}
			
			double[] v=omap.get(p);
			
			for (int i=0;i<v.length;++i){
				v[i]+=Double.parseDouble(items[i+1]);
			}
		}
		s.close();
		
		double[] sumintn=new double[fns.size()];
		

		for (Entry<String, double[]> e:omap.entrySet()){
			
			for (int i=0;i<fns.size();++i) {

				double v=e.getValue()[i+1];
				sumintn[i]+=v;
			}

		}
		
		
		for (Entry<String, double[]> e:omap.entrySet()){
			rppw.print(e.getKey());
			for (int i=0;i<e.getValue().length;++i) {
				double v=e.getValue()[i];
				rppw.print(',');
				rppw.print(v);
			}
			rppw.print(",");

			for (int i=1;i<e.getValue().length;++i) {
				double v=e.getValue()[i];
				double r=1;
				if (i>0) {
					r=sumintn[0]/sumintn[i-1];
				}
				rppw.print(',');
				rppw.print(v*r);
			}
			rppw.println();
		}

		rppw.close();
		
		
	}

	private static void scanHeader(String head) {
		String[] items=head.split("\t");
		for (int i=0;i<items.length;++i) {
			String it=items[i].toLowerCase();
			switch(it) {
			case "filename":
				filename_pos=i;
				break;
			case "decoy":
				decoy_pos=i;
				break;
			case "intensity":
				Intensity_pos=i;
				break;
			case "charge":
				Charge_pos=i;
				break;
			case "proteinname":
				ProteinName_pos=i;
				break;
			}
		}
		
	}
	

	
}

