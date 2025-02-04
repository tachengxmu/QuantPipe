import java.io.File;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Scanner;
import java.util.TreeSet;

public class TricParser {

	static HashMap<String, String> map;

	static {
		map = new HashMap<>();
		map.put("1", "Acetyl");
		map.put("2", "Amidated");
		map.put("3", "Biotin");
		map.put("4", "Carbamidomethyl");
		map.put("5", "Carbamyl");
		map.put("6", "Carboxymethyl");
		map.put("7", "Deamidated");
		map.put("8", "ICAT-G");
		map.put("9", "ICAT-G:2H(8)");
		map.put("10", "Met->Hse");
		map.put("11", "Met->Hsl");
		map.put("12", "ICAT-D:2H(8)");
		map.put("13", "ICAT-D");
		map.put("17", "NIPCAM");
		map.put("20", "PEO-Iodoacetyl-LC-Biotin");
		map.put("21", "Phospho");
		map.put("23", "Dehydrated");
		map.put("24", "Propionamide");
		map.put("25", "Pyridylacetyl");
		map.put("26", "Pyro-carbamidomethyl");
		map.put("27", "Glu->pyro-Glu");
		map.put("28", "Gln->pyro-Glu");
		map.put("29", "SMA");
		map.put("30", "Cation:Na");
		map.put("31", "Pyridylethyl");
		map.put("34", "Methyl");
		map.put("35", "Oxidation");
		map.put("36", "Dimethyl");
		map.put("37", "Trimethyl");
		map.put("39", "Methylthio");
		map.put("40", "Sulfo");
		map.put("41", "Hex");
		map.put("42", "Lipoyl");
		map.put("43", "HexNAc");
		map.put("44", "Farnesyl");
		map.put("45", "Myristoyl");
		map.put("46", "PyridoxalPhosphate");
		map.put("47", "Palmitoyl");
		map.put("48", "GeranylGeranyl");
		map.put("49", "Phosphopantetheine");
		map.put("50", "FAD");
		map.put("51", "Tripalmitate");
		map.put("52", "Guanidinyl");
		map.put("53", "HNE");
		map.put("54", "Glucuronyl");
		map.put("55", "Glutathione");
		map.put("56", "Acetyl:2H(3)");
		map.put("58", "Propionyl");
		map.put("59", "Propionyl:13C(3)");
		map.put("60", "GIST-Quat");
		map.put("61", "GIST-Quat:2H(3)");
		map.put("62", "GIST-Quat:2H(6)");
		map.put("63", "GIST-Quat:2H(9)");
		map.put("64", "Succinyl");
		map.put("65", "Succinyl:2H(4)");
		map.put("66", "Succinyl:13C(4)");
		map.put("357", "probiotinhydrazide");
		map.put("359", "Pro->pyro-Glu");
		map.put("348", "His->Asn");
		map.put("349", "His->Asp");
		map.put("350", "Trp->Hydroxykynurenin");
		map.put("256", "Delta:H(4)C(3)");
		map.put("255", "Delta:H(4)C(2)");
		map.put("368", "Cys->Dha");
		map.put("344", "Arg->GluSA");
		map.put("345", "Trioxidation");
		map.put("89", "Iminobiotin");
		map.put("90", "ESP");
		map.put("91", "ESP:2H(10)");
		map.put("92", "NHS-LC-Biotin");
		map.put("93", "EDT-maleimide-PEO-biotin");
		map.put("94", "IMID");
		map.put("95", "IMID:2H(4)");
		map.put("353", "Lysbiotinhydrazide");
		map.put("97", "Propionamide:2H(3)");
		map.put("354", "Nitro");
		map.put("105", "ICAT-C");
		map.put("254", "Delta:H(2)C(2)");
		map.put("351", "Trp->Kynurenin");
		map.put("352", "Lys->Allysine");
		map.put("106", "ICAT-C:13C(9)");
		map.put("107", "FormylMet");
		map.put("108", "Nethylmaleimide");
		map.put("112", "OxLysBiotinRed");
		map.put("119", "IBTP");
		map.put("113", "OxLysBiotin");
		map.put("114", "OxProBiotinRed");
		map.put("115", "OxProBiotin");
		map.put("116", "OxArgBiotin");
		map.put("117", "OxArgBiotinRed");
		map.put("118", "EDT-iodoacetyl-PEO-biotin");
		map.put("121", "GG");
		map.put("122", "Formyl");
		map.put("123", "ICAT-H");
		map.put("124", "ICAT-H:13C(6)");
		map.put("530", "Cation:K");
		map.put("126", "Thioacyl");
		map.put("127", "Fluoro");
		map.put("128", "Fluorescein");
		map.put("129", "Iodo");
		map.put("130", "Diiodo");
		map.put("131", "Triiodo");
		map.put("134", "Myristoleyl");
		map.put("360", "Pro->Pyrrolidinone");
		map.put("135", "Myristoyl+Delta:H(-4)");
		map.put("136", "Benzoyl");
		map.put("137", "Hex(5)HexNAc(2)");
		map.put("139", "Dansyl");
		map.put("140", "a-type-ion");
		map.put("141", "Amidine");
		map.put("142", "HexNAc(1)dHex(1)");
		map.put("143", "HexNAc(2)");
		map.put("144", "Hex(3)");
		map.put("145", "HexNAc(1)dHex(2)");
		map.put("146", "Hex(1)HexNAc(1)dHex(1)");
		map.put("147", "HexNAc(2)dHex(1)");
		map.put("148", "Hex(1)HexNAc(2)");
		map.put("149", "Hex(1)HexNAc(1)NeuAc(1)");
		map.put("150", "HexNAc(2)dHex(2)");
		map.put("151", "Hex(1)HexNAc(2)Pent(1)");
		map.put("152", "Hex(1)HexNAc(2)dHex(1)");
		map.put("153", "Hex(2)HexNAc(2)");
		map.put("154", "Hex(3)HexNAc(1)Pent(1)");
		map.put("155", "Hex(1)HexNAc(2)dHex(1)Pent(1)");
		map.put("156", "Hex(1)HexNAc(2)dHex(2)");
		map.put("157", "Hex(2)HexNAc(2)Pent(1)");
		map.put("158", "Hex(2)HexNAc(2)dHex(1)");
		map.put("159", "Hex(3)HexNAc(2)");
		map.put("160", "Hex(1)HexNAc(1)NeuAc(2)");
		map.put("161", "Hex(3)HexNAc(2)P(1)");
		map.put("162", "Delta:S(-1)Se(1)");
		map.put("171", "NBS:13C(6)");
		map.put("329", "Methyl:2H(3)13C(1)");
		map.put("330", "Dimethyl:2H(6)13C(2)");
		map.put("172", "NBS");
		map.put("170", "Delta:H(1)N(-1)18O(1)");
		map.put("195", "QAT");
		map.put("176", "BHT");
		map.put("327", "Delta:H(4)C(2)O(-1)S(1)");
		map.put("178", "DAET");
		map.put("369", "Pro->Pyrrolidone");
		map.put("184", "Label:13C(9)");
		map.put("185", "Label:13C(9)+Phospho");
		map.put("188", "Label:13C(6)");
		map.put("186", "HPG");
		map.put("187", "2HPG");
		map.put("196", "QAT:2H(3)");
		map.put("193", "Label:18O(2)");
		map.put("194", "AccQTag");
		map.put("199", "Dimethyl:2H(4)");
		map.put("197", "EQAT");
		map.put("198", "EQAT:2H(5)");
		map.put("200", "Ethanedithiol");
		map.put("212", "NEIAA:2H(5)");
		map.put("205", "Delta:H(6)C(6)O(1)");
		map.put("206", "Delta:H(4)C(3)O(1)");
		map.put("207", "Delta:H(2)C(3)");
		map.put("208", "Delta:H(4)C(6)");
		map.put("209", "Delta:H(8)C(6)O(2)");
		map.put("213", "ADP-Ribosyl");
		map.put("211", "NEIAA");
		map.put("214", "iTRAQ4plex");
		map.put("253", "Crotonaldehyde");
		map.put("340", "Bromo");
		map.put("342", "Amino");
		map.put("343", "Argbiotinhydrazide");
		map.put("258", "Label:18O(1)");
		map.put("259", "Label:13C(6)15N(2)");
		map.put("260", "Thiophospho");
		map.put("261", "SPITC");
		map.put("243", "IGBP");
		map.put("270", "Cytopiloyne");
		map.put("271", "Cytopiloyne+water");
		map.put("267", "Label:13C(6)15N(4)");
		map.put("269", "Label:13C(9)15N(1)");
		map.put("262", "Label:2H(3)");
		map.put("268", "Label:13C(5)15N(1)");
		map.put("264", "PET");
		map.put("272", "CAF");
		map.put("273", "Xlink:SSD");
		map.put("275", "Nitrosyl");
		map.put("276", "AEBS");
		map.put("278", "Ethanolyl");
		map.put("987", "Label:13C(6)15N(2)+Dimethyl");
		map.put("371", "HMVK");
		map.put("280", "Ethyl");
		map.put("281", "CoenzymeA");
		map.put("528", "Methyl+Deamidated");
		map.put("529", "Delta:H(5)C(2)");
		map.put("284", "Methyl:2H(2)");
		map.put("285", "SulfanilicAcid");
		map.put("286", "SulfanilicAcid:13C(6)");
		map.put("289", "Biotin-PEO-Amine");
		map.put("288", "Trp->Oxolactone");
		map.put("290", "Biotin-HPDP");
		map.put("291", "Delta:Hg(1)");
		map.put("292", "IodoU-AMP");
		map.put("293", "CAMthiopropanoyl");
		map.put("294", "IED-Biotin");
		map.put("295", "dHex");
		map.put("298", "Methyl:2H(3)");
		map.put("299", "Carboxy");
		map.put("301", "Bromobimane");
		map.put("302", "Menadione");
		map.put("303", "DeStreak");
		map.put("305", "dHex(1)Hex(3)HexNAc(4)");
		map.put("307", "dHex(1)Hex(4)HexNAc(4)");
		map.put("308", "dHex(1)Hex(5)HexNAc(4)");
		map.put("309", "Hex(3)HexNAc(4)");
		map.put("310", "Hex(4)HexNAc(4)");
		map.put("311", "Hex(5)HexNAc(4)");
		map.put("312", "Cysteinyl");
		map.put("313", "Lys-loss");
		map.put("314", "Nmethylmaleimide");
		map.put("494", "CyDye-Cy3");
		map.put("316", "DimethylpyrroleAdduct");
		map.put("318", "Delta:H(2)C(5)");
		map.put("319", "Delta:H(2)C(3)O(1)");
		map.put("320", "Nethylmaleimide+water");
		map.put("768", "Methyl+Acetyl:2H(3)");
		map.put("323", "Xlink:B10621");
		map.put("324", "DTBP");
		map.put("325", "FP-Biotin");
		map.put("332", "Thiophos-S-S-biotin");
		map.put("333", "Can-FP-biotin");
		map.put("335", "HNE+Delta:H(2)");
		map.put("361", "Thrbiotinhydrazide");
		map.put("337", "Methylamine");
		map.put("362", "Diisopropylphosphate");
		map.put("363", "Isopropylphospho");
		map.put("364", "ICPL:13C(6)");
		map.put("893", "CarbamidomethylDTT");
		map.put("365", "ICPL");
		map.put("366", "Deamidated:18O(1)");
		map.put("372", "Arg->Orn");
		map.put("531", "Cation:Cu[I]");
		map.put("374", "Dehydro");
		map.put("375", "Diphthamide");
		map.put("376", "Hydroxyfarnesyl");
		map.put("377", "Diacylglycerol");
		map.put("378", "Carboxyethyl");
		map.put("379", "Hypusine");
		map.put("380", "Retinylidene");
		map.put("381", "Lys->AminoadipicAcid");
		map.put("382", "Cys->PyruvicAcid");
		map.put("385", "Ammonia-loss");
		map.put("387", "Phycocyanobilin");
		map.put("388", "Phycoerythrobilin");
		map.put("389", "Phytochromobilin");
		map.put("390", "Heme");
		map.put("391", "Molybdopterin");
		map.put("392", "Quinone");
		map.put("393", "Glucosylgalactosyl");
		map.put("394", "GPIanchor");
		map.put("395", "PhosphoribosyldephosphoCoA");
		map.put("396", "GlycerylPE");
		map.put("397", "Triiodothyronine");
		map.put("398", "Thyroxine");
		map.put("400", "Tyr->Dha");
		map.put("401", "Didehydro");
		map.put("402", "Cys->Oxoalanine");
		map.put("403", "Ser->LacticAcid");
		map.put("451", "GluGlu");
		map.put("405", "Phosphoadenosine");
		map.put("450", "Glu");
		map.put("407", "Hydroxycinnamyl");
		map.put("408", "Glycosyl");
		map.put("409", "FMNH");
		map.put("410", "Archaeol");
		map.put("411", "Phenylisocyanate");
		map.put("412", "Phenylisocyanate:2H(5)");
		map.put("413", "Phosphoguanosine");
		map.put("414", "Hydroxymethyl");
		map.put("415", "MolybdopterinGD+Delta:S(-1)Se(1)");
		map.put("416", "Dipyrrolylmethanemethyl");
		map.put("417", "PhosphoUridine");
		map.put("419", "Glycerophospho");
		map.put("420", "Carboxy->Thiocarboxy");
		map.put("421", "Sulfide");
		map.put("422", "PyruvicAcidIminyl");
		map.put("423", "Delta:Se(1)");
		map.put("424", "MolybdopterinGD");
		map.put("425", "Dioxidation");
		map.put("426", "Octanoyl");
		map.put("428", "PhosphoHexNAc");
		map.put("429", "PhosphoHex");
		map.put("431", "Palmitoleyl");
		map.put("432", "Cholesterol");
		map.put("433", "Didehydroretinylidene");
		map.put("434", "CHDH");
		map.put("435", "Methylpyrroline");
		map.put("436", "Hydroxyheme");
		map.put("437", "MicrocinC7");
		map.put("438", "Cyano");
		map.put("439", "Diironsubcluster");
		map.put("440", "Amidino");
		map.put("442", "FMN");
		map.put("443", "FMNC");
		map.put("444", "CuSMo");
		map.put("445", "Hydroxytrimethyl");
		map.put("447", "Deoxy");
		map.put("448", "Microcin");
		map.put("449", "Decanoyl");
		map.put("452", "GluGluGlu");
		map.put("453", "GluGluGluGlu");
		map.put("454", "HexN");
		map.put("455", "Xlink:DMP-s");
		map.put("456", "Xlink:DMP");
		map.put("457", "NDA");
		map.put("464", "SPITC:13C(6)");
		map.put("477", "TMAB:2H(9)");
		map.put("476", "TMAB");
		map.put("478", "FTC");
		map.put("472", "AEC-MAEC");
		map.put("493", "BADGE");
		map.put("481", "Label:2H(4)");
		map.put("490", "Hep");
		map.put("495", "CyDye-Cy5");
		map.put("488", "DHP");
		map.put("498", "BHTOH");
		map.put("499", "IGBP:13C(2)");
		map.put("500", "Nmethylmaleimide+water");
		map.put("501", "PyMIC");
		map.put("503", "LG-lactam-K");
		map.put("519", "BisANS");
		map.put("520", "Piperidine");
		map.put("518", "Diethyl");
		map.put("504", "LG-Hlactam-K");
		map.put("510", "Dimethyl:2H(4)13C(2)");
		map.put("513", "C8-QAT");
		map.put("512", "Hex(2)");
		map.put("505", "LG-lactam-R");
		map.put("1036", "Withaferin");
		map.put("1037", "Biotin:Thermo-88317");
		map.put("525", "CLIP_TRAQ_2");
		map.put("506", "LG-Hlactam-R");
		map.put("522", "Maleimide-PEO2-Biotin");
		map.put("523", "Sulfo-NHS-LC-LC-Biotin");
		map.put("515", "FNEM");
		map.put("514", "PropylNAGthiazoline");
		map.put("526", "Dethiomethyl");
		map.put("532", "iTRAQ4plex114");
		map.put("533", "iTRAQ4plex115");
		map.put("534", "Dibromo");
		map.put("535", "LRGG");
		map.put("536", "CLIP_TRAQ_3");
		map.put("537", "CLIP_TRAQ_4");
		map.put("538", "Biotin:Cayman-10141");
		map.put("539", "Biotin:Cayman-10013");
		map.put("540", "Ala->Ser");
		map.put("541", "Ala->Thr");
		map.put("542", "Ala->Asp");
		map.put("543", "Ala->Pro");
		map.put("544", "Ala->Gly");
		map.put("545", "Ala->Glu");
		map.put("546", "Ala->Val");
		map.put("547", "Cys->Phe");
		map.put("548", "Cys->Ser");
		map.put("549", "Cys->Trp");
		map.put("550", "Cys->Tyr");
		map.put("551", "Cys->Arg");
		map.put("552", "Cys->Gly");
		map.put("553", "Asp->Ala");
		map.put("554", "Asp->His");
		map.put("555", "Asp->Asn");
		map.put("556", "Asp->Gly");
		map.put("557", "Asp->Tyr");
		map.put("558", "Asp->Glu");
		map.put("559", "Asp->Val");
		map.put("560", "Glu->Ala");
		map.put("561", "Glu->Gln");
		map.put("562", "Glu->Asp");
		map.put("563", "Glu->Lys");
		map.put("564", "Glu->Gly");
		map.put("565", "Glu->Val");
		map.put("566", "Phe->Ser");
		map.put("567", "Phe->Cys");
		map.put("568", "Phe->Xle");
		map.put("569", "Phe->Tyr");
		map.put("570", "Phe->Val");
		map.put("571", "Gly->Ala");
		map.put("572", "Gly->Ser");
		map.put("573", "Gly->Trp");
		map.put("574", "Gly->Glu");
		map.put("575", "Gly->Val");
		map.put("576", "Gly->Asp");
		map.put("577", "Gly->Cys");
		map.put("578", "Gly->Arg");
		map.put("698", "dNIC");
		map.put("580", "His->Pro");
		map.put("581", "His->Tyr");
		map.put("582", "His->Gln");
		map.put("697", "NIC");
		map.put("584", "His->Arg");
		map.put("585", "His->Xle");
		map.put("1125", "Xle->Ala");
		map.put("588", "Xle->Thr");
		map.put("589", "Xle->Asn");
		map.put("590", "Xle->Lys");
		map.put("594", "Lys->Thr");
		map.put("595", "Lys->Asn");
		map.put("596", "Lys->Glu");
		map.put("597", "Lys->Gln");
		map.put("598", "Lys->Met");
		map.put("599", "Lys->Arg");
		map.put("600", "Lys->Xle");
		map.put("601", "Xle->Ser");
		map.put("602", "Xle->Phe");
		map.put("603", "Xle->Trp");
		map.put("604", "Xle->Pro");
		map.put("605", "Xle->Val");
		map.put("606", "Xle->His");
		map.put("607", "Xle->Gln");
		map.put("608", "Xle->Met");
		map.put("609", "Xle->Arg");
		map.put("610", "Met->Thr");
		map.put("611", "Met->Arg");
		map.put("613", "Met->Lys");
		map.put("614", "Met->Xle");
		map.put("615", "Met->Val");
		map.put("616", "Asn->Ser");
		map.put("617", "Asn->Thr");
		map.put("618", "Asn->Lys");
		map.put("619", "Asn->Tyr");
		map.put("620", "Asn->His");
		map.put("621", "Asn->Asp");
		map.put("622", "Asn->Xle");
		map.put("623", "Pro->Ser");
		map.put("624", "Pro->Ala");
		map.put("625", "Pro->His");
		map.put("626", "Pro->Gln");
		map.put("627", "Pro->Thr");
		map.put("628", "Pro->Arg");
		map.put("629", "Pro->Xle");
		map.put("630", "Gln->Pro");
		map.put("631", "Gln->Lys");
		map.put("632", "Gln->Glu");
		map.put("633", "Gln->His");
		map.put("634", "Gln->Arg");
		map.put("635", "Gln->Xle");
		map.put("636", "Arg->Ser");
		map.put("637", "Arg->Trp");
		map.put("638", "Arg->Thr");
		map.put("639", "Arg->Pro");
		map.put("640", "Arg->Lys");
		map.put("641", "Arg->His");
		map.put("642", "Arg->Gln");
		map.put("643", "Arg->Met");
		map.put("644", "Arg->Cys");
		map.put("645", "Arg->Xle");
		map.put("646", "Arg->Gly");
		map.put("647", "Ser->Phe");
		map.put("648", "Ser->Ala");
		map.put("649", "Ser->Trp");
		map.put("650", "Ser->Thr");
		map.put("651", "Ser->Asn");
		map.put("652", "Ser->Pro");
		map.put("653", "Ser->Tyr");
		map.put("654", "Ser->Cys");
		map.put("655", "Ser->Arg");
		map.put("656", "Ser->Xle");
		map.put("657", "Ser->Gly");
		map.put("658", "Thr->Ser");
		map.put("659", "Thr->Ala");
		map.put("660", "Thr->Asn");
		map.put("661", "Thr->Lys");
		map.put("662", "Thr->Pro");
		map.put("663", "Thr->Met");
		map.put("664", "Thr->Xle");
		map.put("665", "Thr->Arg");
		map.put("666", "Val->Phe");
		map.put("667", "Val->Ala");
		map.put("668", "Val->Glu");
		map.put("669", "Val->Met");
		map.put("670", "Val->Asp");
		map.put("671", "Val->Xle");
		map.put("672", "Val->Gly");
		map.put("673", "Trp->Ser");
		map.put("674", "Trp->Cys");
		map.put("675", "Trp->Arg");
		map.put("676", "Trp->Gly");
		map.put("677", "Trp->Xle");
		map.put("678", "Tyr->Phe");
		map.put("679", "Tyr->Ser");
		map.put("680", "Tyr->Asn");
		map.put("681", "Tyr->His");
		map.put("682", "Tyr->Asp");
		map.put("683", "Tyr->Cys");
		map.put("684", "BDMAPP");
		map.put("685", "NA-LNO2");
		map.put("686", "NA-OA-NO2");
		map.put("687", "ICPL:2H(4)");
		map.put("894", "CarboxymethylDTT");
		map.put("730", "iTRAQ8plex");
		map.put("695", "Label:13C(6)15N(1)");
		map.put("696", "Label:2H(9)13C(6)15N(2)");
		map.put("720", "HNE-Delta:H(2)O");
		map.put("721", "4-ONE");
		map.put("723", "O-Dimethylphosphate");
		map.put("724", "O-Methylphosphate");
		map.put("725", "Diethylphosphate");
		map.put("726", "Ethylphosphate");
		map.put("727", "O-pinacolylmethylphosphonate");
		map.put("728", "Methylphosphonate");
		map.put("729", "O-Isopropylmethylphosphonate");
		map.put("731", "iTRAQ8plex:13C(6)15N(2)");
		map.put("735", "DTT_ST");
		map.put("734", "Ethanolamine");
		map.put("737", "TMT6plex");
		map.put("736", "DTT_C");
		map.put("738", "TMT2plex");
		map.put("739", "TMT");
		map.put("740", "ExacTagThiol");
		map.put("741", "ExacTagAmine");
		map.put("744", "NO_SMX_SEMD");
		map.put("743", "4-ONE+Delta:H(-2)O(-1)");
		map.put("745", "NO_SMX_SMCT");
		map.put("746", "NO_SMX_SIMD");
		map.put("747", "Malonyl");
		map.put("748", "3sulfo");
		map.put("750", "trifluoro");
		map.put("751", "TNBS");
		map.put("774", "Biotin-phenacyl");
		map.put("764", "DTT_C:2H(6)");
		map.put("771", "lapachenole");
		map.put("772", "Label:13C(5)");
		map.put("773", "maleimide");
		map.put("762", "IDEnT");
		map.put("763", "DTT_ST:2H(6)");
		map.put("765", "Met-loss");
		map.put("766", "Met-loss+Acetyl");
		map.put("767", "Menadione-HQ");
		map.put("775", "Carboxymethyl:13C(2)");
		map.put("776", "NEM:2H(5)");
		map.put("822", "Gly-loss+Amide");
		map.put("827", "TMPP-Ac");
		map.put("799", "Label:13C(6)+GG");
		map.put("837", "Arg->Npo");
		map.put("834", "Label:2H(4)+Acetyl");
		map.put("801", "Pentylamine");
		map.put("800", "Biotin:Thermo-21345");
		map.put("830", "Dihydroxyimidazolidine");
		map.put("825", "DFDNB");
		map.put("821", "Cy3b-maleimide");
		map.put("793", "Hex1HexNAc1");
		map.put("792", "AEC-MAEC:2H(4)");
		map.put("824", "BMOE");
		map.put("811", "Biotin:Thermo-21360");
		map.put("835", "Label:13C(6)+Acetyl");
		map.put("836", "Label:13C(6)15N(2)+Acetyl");
		map.put("846", "EQIGG");
		map.put("849", "cGMP");
		map.put("851", "cGMP+RMP-loss");
		map.put("888", "mTRAQ");
		map.put("848", "Arg2PG");
		map.put("853", "Label:2H(4)+GG");
		map.put("854", "Label:13C(8)15N(2)");
		map.put("862", "Label:13C(1)2H(3)");
		map.put("861", "ZGB");
		map.put("859", "MG-H1");
		map.put("860", "G-H1");
		map.put("864", "Label:13C(6)15N(2)+GG");
		map.put("866", "ICPL:13C(6)2H(4)");
		map.put("890", "DyLight-maleimide");
		map.put("889", "mTRAQ:13C(3)15N(1)");
		map.put("891", "Methyl-PEO12-Maleimide");
		map.put("887", "MDCC");
		map.put("877", "QQQTGG");
		map.put("876", "QEQTGG");
		map.put("886", "HydroxymethylOP");
		map.put("884", "Biotin:Thermo-21325");
		map.put("885", "Label:13C(1)2H(3)+Oxidation");
		map.put("878", "Bodipy");
		map.put("895", "Biotin-PEG-PRA");
		map.put("896", "Met->Aha");
		map.put("897", "Label:15N(4)");
		map.put("898", "pyrophospho");
		map.put("899", "Met->Hpg");
		map.put("901", "4AcAllylGal");
		map.put("902", "DimethylArsino");
		map.put("903", "Lys->CamCys");
		map.put("904", "Phe->CamCys");
		map.put("905", "Leu->MetOx");
		map.put("906", "Lys->MetOx");
		map.put("907", "Galactosyl");
		map.put("908", "SMCC-maleimide");
		map.put("910", "Bacillosamine");
		map.put("911", "MTSL");
		map.put("912", "HNE-BAHAH");
		map.put("915", "Ethoxyformyl");
		map.put("914", "Methylmalonylation");
		map.put("938", "AROD");
		map.put("939", "Cys->methylaminoAla");
		map.put("940", "Cys->ethylaminoAla");
		map.put("923", "Label:13C(4)15N(2)+GG");
		map.put("926", "ethylamino");
		map.put("928", "MercaptoEthanol");
		map.put("935", "Atto495Maleimide");
		map.put("934", "AMTzHexNAc2");
		map.put("931", "Ethyl+Deamidated");
		map.put("932", "VFQQQTGG");
		map.put("933", "VIEVYQEQTGG");
		map.put("936", "Chlorination");
		map.put("937", "dichlorination");
		map.put("941", "DNPS");
		map.put("942", "SulfoGMBS");
		map.put("943", "DimethylamineGMBS");
		map.put("944", "Label:15N(2)2H(9)");
		map.put("946", "LG-anhydrolactam");
		map.put("947", "LG-pyrrole");
		map.put("948", "LG-anhyropyrrole");
		map.put("949", "3-deoxyglucosone");
		map.put("950", "Cation:Li");
		map.put("951", "Cation:Ca[II]");
		map.put("952", "Cation:Fe[II]");
		map.put("953", "Cation:Ni[II]");
		map.put("954", "Cation:Zn[II]");
		map.put("955", "Cation:Ag");
		map.put("956", "Cation:Mg[II]");
		map.put("957", "2-succinyl");
		map.put("958", "Propargylamine");
		map.put("959", "Phosphopropargyl");
		map.put("960", "SUMO2135");
		map.put("961", "SUMO3549");
		map.put("975", "Chlorpyrifos");
		map.put("978", "BITC");
		map.put("977", "Carbofuran");
		map.put("979", "PEITC");
		map.put("967", "thioacylPA");
		map.put("971", "maleimide3");
		map.put("972", "maleimide5");
		map.put("973", "Puromycin");
		map.put("981", "glucosone");
		map.put("986", "Label:13C(6)+Dimethyl");
		map.put("984", "cysTMT");
		map.put("985", "cysTMT6plex");
		map.put("991", "ISD_z+2_ion");
		map.put("989", "Ammonium");
		map.put("998", "BHAc");
		map.put("993", "Biotin:Sigma-B1267");
		map.put("994", "Label:15N(1)");
		map.put("995", "Label:15N(2)");
		map.put("996", "Label:15N(3)");
		map.put("997", "sulfo+amino");
		map.put("1000", "AHA-Alkyne");
		map.put("1001", "AHA-Alkyne-KDDDD");
		map.put("1002", "EGCG1");
		map.put("1003", "EGCG2");
		map.put("1004", "Label:13C(6)15N(4)+Methyl");
		map.put("1005", "Label:13C(6)15N(4)+Dimethyl");
		map.put("1006", "Label:13C(6)15N(4)+Methyl:2H(3)13C(1)");
		map.put("1007", "Label:13C(6)15N(4)+Dimethyl:2H(6)13C(2)");
		map.put("1008", "SecCarbamidomethyl");
		map.put("1009", "Thiazolidine");
		map.put("1010", "DEDGFLYMVYASQETFG");
		map.put("1012", "Biotin:Invitrogen-M1602");
		map.put("1020", "Xlink:DSS");
		map.put("1017", "DMPO");
		map.put("1014", "glycidamide");
		map.put("1015", "Ahx2+Hsl");
		map.put("1018", "ICDID");
		map.put("1019", "ICDID:2H(6)");
		map.put("1021", "Xlink:EGS");
		map.put("1022", "Xlink:DST");
		map.put("1023", "Xlink:DTSSP");
		map.put("1024", "Xlink:SMCC");
		map.put("1032", "2-nitrobenzyl");
		map.put("1027", "Xlink:DMP-de");
		map.put("1028", "Xlink:EGScleaved");
		map.put("1033", "SecNEM");
		map.put("1034", "SecNEM:2H(5)");
		map.put("1035", "Thiadiazole");
		map.put("1031", "Biotin:Thermo-88310");
		map.put("1038", "TAMRA-FP");
		map.put("1039", "Biotin:Thermo-21901+H2O");
		map.put("1041", "Deoxyhypusine");
		map.put("1042", "Acetyldeoxyhypusine");
		map.put("1043", "Acetylhypusine");
		map.put("1044", "Ala->Cys");
		map.put("1045", "Ala->Phe");
		map.put("1046", "Ala->His");
		map.put("1047", "Ala->Xle");
		map.put("1048", "Ala->Lys");
		map.put("1049", "Ala->Met");
		map.put("1050", "Ala->Asn");
		map.put("1051", "Ala->Gln");
		map.put("1052", "Ala->Arg");
		map.put("1053", "Ala->Trp");
		map.put("1054", "Ala->Tyr");
		map.put("1055", "Cys->Ala");
		map.put("1056", "Cys->Asp");
		map.put("1057", "Cys->Glu");
		map.put("1058", "Cys->His");
		map.put("1059", "Cys->Xle");
		map.put("1060", "Cys->Lys");
		map.put("1061", "Cys->Met");
		map.put("1062", "Cys->Asn");
		map.put("1063", "Cys->Pro");
		map.put("1064", "Cys->Gln");
		map.put("1065", "Cys->Thr");
		map.put("1066", "Cys->Val");
		map.put("1067", "Asp->Cys");
		map.put("1068", "Asp->Phe");
		map.put("1069", "Asp->Xle");
		map.put("1070", "Asp->Lys");
		map.put("1071", "Asp->Met");
		map.put("1072", "Asp->Pro");
		map.put("1073", "Asp->Gln");
		map.put("1074", "Asp->Arg");
		map.put("1075", "Asp->Ser");
		map.put("1076", "Asp->Thr");
		map.put("1077", "Asp->Trp");
		map.put("1078", "Glu->Cys");
		map.put("1079", "Glu->Phe");
		map.put("1080", "Glu->His");
		map.put("1081", "Glu->Xle");
		map.put("1082", "Glu->Met");
		map.put("1083", "Glu->Asn");
		map.put("1084", "Glu->Pro");
		map.put("1085", "Glu->Arg");
		map.put("1086", "Glu->Ser");
		map.put("1087", "Glu->Thr");
		map.put("1088", "Glu->Trp");
		map.put("1089", "Glu->Tyr");
		map.put("1090", "Phe->Ala");
		map.put("1091", "Phe->Asp");
		map.put("1092", "Phe->Glu");
		map.put("1093", "Phe->Gly");
		map.put("1094", "Phe->His");
		map.put("1095", "Phe->Lys");
		map.put("1096", "Phe->Met");
		map.put("1097", "Phe->Asn");
		map.put("1098", "Phe->Pro");
		map.put("1099", "Phe->Gln");
		map.put("1100", "Phe->Arg");
		map.put("1101", "Phe->Thr");
		map.put("1102", "Phe->Trp");
		map.put("1103", "Gly->Phe");
		map.put("1104", "Gly->His");
		map.put("1105", "Gly->Xle");
		map.put("1106", "Gly->Lys");
		map.put("1107", "Gly->Met");
		map.put("1108", "Gly->Asn");
		map.put("1109", "Gly->Pro");
		map.put("1110", "Gly->Gln");
		map.put("1111", "Gly->Thr");
		map.put("1112", "Gly->Tyr");
		map.put("1113", "His->Ala");
		map.put("1114", "His->Cys");
		map.put("1115", "His->Glu");
		map.put("1116", "His->Phe");
		map.put("1117", "His->Gly");
		map.put("1119", "His->Lys");
		map.put("1120", "His->Met");
		map.put("1121", "His->Ser");
		map.put("1122", "His->Thr");
		map.put("1123", "His->Val");
		map.put("1124", "His->Trp");
		map.put("1126", "Xle->Cys");
		map.put("1127", "Xle->Asp");
		map.put("1128", "Xle->Glu");
		map.put("1129", "Xle->Gly");
		map.put("1130", "Xle->Tyr");
		map.put("1131", "Lys->Ala");
		map.put("1132", "Lys->Cys");
		map.put("1133", "Lys->Asp");
		map.put("1134", "Lys->Phe");
		map.put("1135", "Lys->Gly");
		map.put("1136", "Lys->His");
		map.put("1137", "Lys->Pro");
		map.put("1138", "Lys->Ser");
		map.put("1139", "Lys->Val");
		map.put("1140", "Lys->Trp");
		map.put("1141", "Lys->Tyr");
		map.put("1142", "Met->Ala");
		map.put("1143", "Met->Cys");
		map.put("1144", "Met->Asp");
		map.put("1145", "Met->Glu");
		map.put("1146", "Met->Phe");
		map.put("1147", "Met->Gly");
		map.put("1148", "Met->His");
		map.put("1149", "Met->Asn");
		map.put("1150", "Met->Pro");
		map.put("1151", "Met->Gln");
		map.put("1152", "Met->Ser");
		map.put("1153", "Met->Trp");
		map.put("1154", "Met->Tyr");
		map.put("1155", "Asn->Ala");
		map.put("1156", "Asn->Cys");
		map.put("1157", "Asn->Glu");
		map.put("1158", "Asn->Phe");
		map.put("1159", "Asn->Gly");
		map.put("1160", "Asn->Met");
		map.put("1161", "Asn->Pro");
		map.put("1162", "Asn->Gln");
		map.put("1163", "Asn->Arg");
		map.put("1164", "Asn->Val");
		map.put("1165", "Asn->Trp");
		map.put("1166", "Pro->Cys");
		map.put("1167", "Pro->Asp");
		map.put("1168", "Pro->Glu");
		map.put("1169", "Pro->Phe");
		map.put("1170", "Pro->Gly");
		map.put("1171", "Pro->Lys");
		map.put("1172", "Pro->Met");
		map.put("1173", "Pro->Asn");
		map.put("1174", "Pro->Val");
		map.put("1175", "Pro->Trp");
		map.put("1176", "Pro->Tyr");
		map.put("1177", "Gln->Ala");
		map.put("1178", "Gln->Cys");
		map.put("1179", "Gln->Asp");
		map.put("1180", "Gln->Phe");
		map.put("1181", "Gln->Gly");
		map.put("1182", "Gln->Met");
		map.put("1183", "Gln->Asn");
		map.put("1184", "Gln->Ser");
		map.put("1185", "Gln->Thr");
		map.put("1186", "Gln->Val");
		map.put("1187", "Gln->Trp");
		map.put("1188", "Gln->Tyr");
		map.put("1189", "Arg->Ala");
		map.put("1190", "Arg->Asp");
		map.put("1191", "Arg->Glu");
		map.put("1192", "Arg->Asn");
		map.put("1193", "Arg->Val");
		map.put("1194", "Arg->Tyr");
		map.put("1195", "Arg->Phe");
		map.put("1196", "Ser->Asp");
		map.put("1197", "Ser->Glu");
		map.put("1198", "Ser->His");
		map.put("1199", "Ser->Lys");
		map.put("1200", "Ser->Met");
		map.put("1201", "Ser->Gln");
		map.put("1202", "Ser->Val");
		map.put("1203", "Thr->Cys");
		map.put("1204", "Thr->Asp");
		map.put("1205", "Thr->Glu");
		map.put("1206", "Thr->Phe");
		map.put("1207", "Thr->Gly");
		map.put("1208", "Thr->His");
		map.put("1209", "Thr->Gln");
		map.put("1210", "Thr->Val");
		map.put("1211", "Thr->Trp");
		map.put("1212", "Thr->Tyr");
		map.put("1213", "Val->Cys");
		map.put("1214", "Val->His");
		map.put("1215", "Val->Lys");
		map.put("1216", "Val->Asn");
		map.put("1217", "Val->Pro");
		map.put("1218", "Val->Gln");
		map.put("1219", "Val->Arg");
		map.put("1220", "Val->Ser");
		map.put("1221", "Val->Thr");
		map.put("1222", "Val->Trp");
		map.put("1223", "Val->Tyr");
		map.put("1224", "Trp->Ala");
		map.put("1225", "Trp->Asp");
		map.put("1226", "Trp->Glu");
		map.put("1227", "Trp->Phe");
		map.put("1228", "Trp->His");
		map.put("1229", "Trp->Lys");
		map.put("1230", "Trp->Met");
		map.put("1231", "Trp->Asn");
		map.put("1232", "Trp->Pro");
		map.put("1233", "Trp->Gln");
		map.put("1234", "Trp->Thr");
		map.put("1235", "Trp->Val");
		map.put("1236", "Trp->Tyr");
		map.put("1237", "Tyr->Ala");
		map.put("1238", "Tyr->Glu");
		map.put("1239", "Tyr->Gly");
		map.put("1240", "Tyr->Lys");
		map.put("1241", "Tyr->Met");
		map.put("1242", "Tyr->Pro");
		map.put("1243", "Tyr->Gln");
		map.put("1244", "Tyr->Arg");
		map.put("1245", "Tyr->Thr");
		map.put("1246", "Tyr->Val");
		map.put("1247", "Tyr->Trp");
		map.put("1248", "Tyr->Xle");
		map.put("1249", "AHA-SS");
		map.put("1250", "AHA-SS_CAM");
		map.put("1251", "Biotin:Thermo-33033");
		map.put("1252", "Biotin:Thermo-33033-H");
		map.put("1253", "2-monomethylsuccinyl");
		map.put("1254", "Saligenin");
		map.put("1255", "Cresylphosphate");
		map.put("1256", "CresylSaligeninPhosphate");
		map.put("1257", "Ub-Br2");
		map.put("1258", "Ub-VME");
		map.put("1260", "Ub-amide");
		map.put("1261", "Ub-fluorescein");
		map.put("1262", "2-dimethylsuccinyl");
		map.put("1263", "Gly");
		map.put("1264", "pupylation");
		map.put("1266", "Label:13C(4)");
		map.put("1271", "HCysteinyl");
		map.put("1267", "Label:13C(4)+Oxidation");
		map.put("1276", "UgiJoullie");
		map.put("1270", "HCysThiolactone");
		map.put("1282", "UgiJoullieProGly");
		map.put("1277", "Dipyridyl");
		map.put("1278", "Furan");
		map.put("1279", "Difuran");
		map.put("1281", "BMP-piperidinol");
		map.put("1283", "UgiJoullieProGlyProGly");
		map.put("1287", "Arg-loss");
		map.put("1288", "Arg");
		map.put("1286", "IMEHex(2)NeuAc(1)");
		map.put("1289", "Butyryl");
		map.put("1290", "Dicarbamidomethyl");
		map.put("1291", "Dimethyl:2H(6)");
		map.put("1292", "GGQ");
		map.put("1293", "QTGG");
		map.put("1297", "Label:13C(3)15N(1)");
		map.put("1296", "Label:13C(3)");
		map.put("1298", "Label:13C(4)15N(1)");
		map.put("1299", "Label:2H(10)");
		map.put("1300", "Label:2H(4)13C(1)");
		map.put("1301", "Lys");
		map.put("1302", "mTRAQ:13C(6)15N(2)");
		map.put("1303", "NeuAc");
		map.put("1304", "NeuGc");
		map.put("1305", "Propyl");
		map.put("1306", "Propyl:2H(6)");
		map.put("1310", "Propiophenone");
		map.put("1345", "PS_Hapten");
		map.put("1348", "Cy3-maleimide");
		map.put("1312", "Delta:H(6)C(3)O(1)");
		map.put("1313", "Delta:H(8)C(6)O(1)");
		map.put("1314", "biotinAcrolein298");
		map.put("1315", "MM-diphenylpentanone");
		map.put("1317", "EHD-diphenylpentanone");
		map.put("1349", "benzylguanidine");
		map.put("1350", "CarboxymethylDMAP");
		map.put("1320", "Biotin:Thermo-21901+2H2O");
		map.put("1321", "DiLeu4plex115");
		map.put("1322", "DiLeu4plex");
		map.put("1323", "DiLeu4plex117");
		map.put("1324", "DiLeu4plex118");
		map.put("1330", "bisANS-sulfonates");
		map.put("1331", "DNCB_hapten");
		map.put("1326", "NEMsulfur");
		map.put("1327", "SulfurDioxide");
		map.put("1328", "NEMsulfurWater");
		map.put("1389", "HN3_mustard");
		map.put("1387", "3-phosphoglyceryl");
		map.put("1388", "HN2_mustard");
		map.put("1358", "NEM:2H(5)+H2O");
		map.put("1363", "Crotonyl");
		map.put("1364", "O-Et-N-diMePhospho");
		map.put("1365", "N-dimethylphosphate");
		map.put("1356", "phosphoRibosyl");
		map.put("1355", "azole");
		map.put("1340", "Biotin:Thermo-21911");
		map.put("1341", "iodoTMT");
		map.put("1342", "iodoTMT6plex");
		map.put("1343", "Gluconoylation");
		map.put("1344", "Phosphogluconoylation");
		map.put("1368", "Methyl:2H(3)+Acetyl:2H(3)");
		map.put("1367", "dHex(1)Hex(1)");
		map.put("1380", "methylsulfonylethyl");
		map.put("1370", "Label:2H(3)+Oxidation");
		map.put("1371", "Trimethyl:2H(9)");
		map.put("1372", "Acetyl:13C(2)");
		map.put("1375", "dHex(1)Hex(2)");
		map.put("1376", "dHex(1)Hex(3)");
		map.put("1377", "dHex(1)Hex(4)");
		map.put("1378", "dHex(1)Hex(5)");
		map.put("1379", "dHex(1)Hex(6)");
		map.put("1381", "ethylsulfonylethyl");
		map.put("1382", "phenylsulfonylethyl");
		map.put("1383", "PyridoxalPhosphateH2");
		map.put("1384", "Homocysteic_acid");
		map.put("1385", "Hydroxamic_acid");
		map.put("1390", "Oxidation+NEM");
		map.put("1391", "NHS-fluorescein");
		map.put("1392", "DiART6plex");
		map.put("1393", "DiART6plex115");
		map.put("1394", "DiART6plex116/119");
		map.put("1395", "DiART6plex117");
		map.put("1396", "DiART6plex118");
		map.put("1397", "Iodoacetanilide");
		map.put("1398", "Iodoacetanilide:13C(6)");
		map.put("1399", "Dap-DSP");
		map.put("1400", "MurNAc");
		map.put("1405", "EEEDVIEVYQEQTGG");
		map.put("1402", "Label:2H(7)15N(4)");
		map.put("1403", "Label:2H(6)15N(1)");
		map.put("1406", "EDEDTIDVFQQQTGG");
		map.put("1408", "Hex(5)HexNAc(4)NeuAc(2)");
		map.put("1409", "Hex(5)HexNAc(4)NeuAc(1)");
		map.put("1410", "dHex(1)Hex(5)HexNAc(4)NeuAc(1)");
		map.put("1411", "dHex(1)Hex(5)HexNAc(4)NeuAc(2)");
		map.put("1414", "Trimethyl:13C(3)2H(9)");

	}

	static int decoy_pos=1;
	static int fullPepSeq_pos=12;
	static int pepSeq_pos=11;
	static int charge_pos=13;
	static int protein_pos=26;
	static int mz_pos=14;
			
	public static void main(String[] args) throws FileNotFoundException {
		System.out.println("Usage : java TricParser aligned.tsv db.fasta");
		if (args.length != 2||!new File(args[0]).exists()||!new File(args[1]).exists()) {			
			return;
		}

		HashMap<String,String> dbmap=new HashMap<>();
		
		
		do {
			Scanner sdb=new Scanner(new File(args[1]));
			String pep=null;
			StringBuffer seq=null;
			while (sdb.hasNextLine()) {

				String line=sdb.nextLine();			
				if (line.startsWith(">")){
					if (pep!=null) {
						dbmap.put(pep,seq.toString());
					}
					pep=line.substring(1).split(" ")[0];
					seq=new StringBuffer();
				}
				else {
					seq.append(line);
				}
			}
			dbmap.put(pep,seq.toString().replaceAll("I", "L").replaceAll("[BJOUXZ]", ""));
			sdb.close();
		}while (false);

		TreeSet<String> simple = new TreeSet<>();

		Scanner s = new Scanner(new File(args[0]));

		PrintWriter pw = new PrintWriter(args[0] + ".new");

		PrintWriter pws = new PrintWriter(args[0] + "_pep.csv");
		pws.println("ProteinName,Sequence,SequenceWithMod,SequenceWithModName,mz,Charge,AminoAcidWithMod,ModSite");

		String head=s.nextLine();
		
		scanHeader(head);
		
		pw.print(head);
		pw.println("\tFullPeptideNameWithModName");
		
		while (s.hasNextLine()) {
			String Line=s.nextLine();
			if (Line.isEmpty()){
				continue;
			}
			String[] line = Line.split("\t");
			if (!line[decoy_pos].equals("1")) {

				String key = line[fullPepSeq_pos]+"_"+line[charge_pos];

				if (!simple.contains(key)) {

					simple.add(key);

					StringBuffer simpleBuf = new StringBuffer();
					
					ArrayList<Integer> mpos=new ArrayList<>();

					simpleBuf.append(line[protein_pos]);
					simpleBuf.append(",");
					
					simpleBuf.append(line[pepSeq_pos]);
					simpleBuf.append(",");
					
					simpleBuf.append(line[fullPepSeq_pos]);
					simpleBuf.append(",");
					
					simpleBuf.append(key2mod(line[fullPepSeq_pos],mpos));
					simpleBuf.append(",");
					
					simpleBuf.append(line[mz_pos]);
					simpleBuf.append(",");
					
					simpleBuf.append(line[charge_pos]);
					simpleBuf.append(",");
					
					if (!mpos.isEmpty()) {
						for (int pos:mpos) {
							simpleBuf.append(line[pepSeq_pos].charAt(pos-1));
							simpleBuf.append(";");
						}
						simpleBuf.append("/");
						for (int pos:mpos) {
							simpleBuf.append(pos);
							simpleBuf.append(";");
						}
					}
					simpleBuf.append(",");
					

					if (!mpos.isEmpty()) { 
						String[] protArr = line[protein_pos].split("[;/]");
						for (int i=0;i<protArr.length;++i) {
							if (protArr[i].replaceAll("[0-9]", "").isEmpty()) {
								continue;
							}
							String protSeq=dbmap.get(protArr[i]);
                            if (protSeq==null)
                            {
                                System.err.println(protArr[i]);
                                continue;
                            }
							int idx=protSeq.indexOf(line[pepSeq_pos].replaceAll("I", "L").replaceAll("[BJOUXZ]", ""));
							for (int pos:mpos) {
								simpleBuf.append(pos+idx);
								simpleBuf.append(";");
							}
							if (i<protArr.length-1) {
								simpleBuf.append("/");
							}
						}
					}
					
					pws.println(simpleBuf.toString());
				}
			}

			//StringBuffer sb = new StringBuffer();
			for (int i = 0; i < line.length; ++i) {
				pw.print(line[i]);
				pw.print("\t");
			}
			pw.println(key2mod(line[fullPepSeq_pos],null));
			
			//pw.println(sb.toString());
		}
		s.close();

		pw.close();
		pws.close();
		
		new File(args[0]).delete();
		new File(args[0] + ".new").renameTo(new File(args[0]));
	

	}

	private static void scanHeader(String head) {
		String[] items=head.split("\t");
		for (int i=0;i<items.length;++i) {
			String it=items[i].toLowerCase();
			switch(it) {
			case "decoy":
				decoy_pos=i;
				break;
			case "sequence":
				pepSeq_pos=i;
				break;
			case "fullpeptidename":
				fullPepSeq_pos=i;
				break;
			case "charge":
				charge_pos=i;
				break;
			case "mz":
				mz_pos=i;
				break;
			case "proteinname":
				protein_pos=i;
				break;
			}
		}
		
	}
	
	private static String key2mod(String key,ArrayList<Integer> pos) {
		//int p = key.lastIndexOf("_");
		//key = key.substring(0, p);

		String[] mod = key.split("[()]");

		StringBuffer sb = new StringBuffer();
		
		StringBuffer tmp=new StringBuffer();

		for (int i = 0; i < mod.length; ++i) {
			if (mod[i].toLowerCase().contains("unimod")) {
				String k = mod[i].replaceAll("[^0-9]", "");
				sb.append("(");
				sb.append(map.get(k));
				sb.append(")");
				if (pos!=null) {
					if (k.equals("4")&&mod[i-1].endsWith("C")) {
						continue;
					}
					if (k.equals("35")&&mod[i-1].endsWith("M")) {
						continue;
					}
					if (k.equals("1")&&mod[i-1].endsWith(".")) {
						continue;
					}
					if (tmp.length()>0) {
						pos.add(tmp.length());
					}
				}
			} else {
				if (mod[i].length() != mod[i].replaceAll("[^A-Zcn.]", "").length()) {
					sb.append("(");
					sb.append(mod[i]);
					sb.append(")");
					if (pos!=null) {
						if (mod[i].equals("Carbamidomethyl")&&mod[i-1].endsWith("C")){
							continue;
						}
						if (mod[i].equals("Oxidation")&&mod[i-1].endsWith("M")) {
							continue;
						}
						if (mod[i].equals("Acetyl")&&mod[i-1].endsWith(".")) {
							continue;
						}
						if (tmp.length()>0) {
							pos.add(tmp.length());
						}
					}
				} else {
					sb.append(mod[i]);
					tmp.append(mod[i].replaceAll("[.]", ""));
				}
			}
		}

		return sb.toString();
	}

}
