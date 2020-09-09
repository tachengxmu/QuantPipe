using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace QuantPipe
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            run_ini_template = run_ini_template.Replace('\'', '"');
            run_ini = run_ini_template;
        }

        string wd = null;

        string run_ini_template = @"<?xml version='1.0' encoding='ISO-8859-1'?>
<PARAMETERS version='1.7.0' xsi:noNamespaceSchemaLocation='https://raw.githubusercontent.com/OpenMS/OpenMS/develop/share/OpenMS/SCHEMAS/Param_1_7_0.xsd' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
  <NODE name='OpenSwathAssayGenerator' description='Generates assays according to different models for a specific TraML'>
    <NODE name='1' description='Instance &apos;1&apos; section for &apos;OpenSwathAssayGenerator&apos;'>
      <ITEM name='min_transitions' value='6' type='int' description='minimal number of transitions' required='false' advanced='false' />
      <ITEM name='max_transitions' value='6' type='int' description='maximal number of transitions' required='false' advanced='false' />
      <ITEM name='allowed_fragment_types' value='b,y' type='string' description='allowed fragment types' required='false' advanced='false' />
      <ITEM name='allowed_fragment_charges' value='1,2,3,4' type='string' description='allowed fragment charge states' required='false' advanced='false' />
      <ITEM name='enable_detection_specific_losses' value='false' type='bool' description='set this flag if specific neutral losses for detection fragment ions should be allowed' required='false' advanced='false' />
      <ITEM name='enable_detection_unspecific_losses' value='false' type='bool' description='set this flag if unspecific neutral losses (H2O1, H3N1, C1H2N2, C1H2N1O1) for detection fragment ions should be allowed' required='false' advanced='false' />
      <ITEM name='precursor_mz_threshold' value='0.025' type='double' description='MZ threshold in Thomson for precursor ion selection' required='false' advanced='false' />
      <ITEM name='precursor_lower_mz_limit' value='400.0' type='double' description='lower MZ limit for precursor ions' required='false' advanced='false' />
      <ITEM name='precursor_upper_mz_limit' value='1200.0' type='double' description='upper MZ limit for precursor ions' required='false' advanced='false' />
      <ITEM name='product_mz_threshold' value='0.025' type='double' description='MZ threshold in Thomson for fragment ion annotation' required='false' advanced='false' />
      <ITEM name='product_lower_mz_limit' value='350.0' type='double' description='lower MZ limit for fragment ions' required='false' advanced='false' />
      <ITEM name='product_upper_mz_limit' value='2000.0' type='double' description='upper MZ limit for fragment ions' required='false' advanced='false' />
      <ITEM name='max_num_alternative_localizations' value='10000' type='int' description='IPF: maximum number of site-localization permutations' required='false' advanced='true' />
      <ITEM name='disable_identification_ms2_precursors' value='false' type='bool' description='IPF: set this flag if MS2-level precursor ions for identification should not be allowed for extraction of the precursor signal from the fragment ion data (MS2-level).' required='false' advanced='true' />
      <ITEM name='disable_identification_specific_losses' value='false' type='bool' description='IPF: set this flag if specific neutral losses for identification fragment ions should not be allowed' required='false' advanced='true' />
      <ITEM name='enable_identification_unspecific_losses' value='false' type='bool' description='IPF: set this flag if unspecific neutral losses (H2O1, H3N1, C1H2N2, C1H2N1O1) for identification fragment ions should be allowed' required='false' advanced='true' />
      <ITEM name='enable_swath_specifity' value='false' type='bool' description='IPF: set this flag if identification transitions without precursor specificity (i.e. across whole precursor isolation window instead of precursor MZ) should be generated.' required='false' advanced='true' />
      <ITEM name='log' value='' type='string' description='Name of log file (created only when specified)' required='false' advanced='true' />
      <ITEM name='debug' value='0' type='int' description='Sets the debug level' required='false' advanced='true' />
      <ITEM name='threads' value='1' type='int' description='Sets the number of threads allowed to be used by the TOPP tool' required='false' advanced='false' />
      <ITEM name='no_progress' value='false' type='bool' description='Disables progress logging to command line' required='false' advanced='true' />
      <ITEM name='force' value='false' type='bool' description='Overwrite tool specific checks.' required='false' advanced='true' />
      <ITEM name='test' value='false' type='bool' description='Enables the test mode (needed for internal use only)' required='false' advanced='true' />
    </NODE>
  </NODE>


  <NODE name='OpenSwathDecoyGenerator' description='Generates decoys according to different models for a specific TraML'>
    <NODE name='1' description='Instance &apos;1&apos; section for &apos;OpenSwathDecoyGenerator&apos;'>    
      <ITEM name='method' value='shuffle' type='string' description='decoy generation method (&apos;shuffle&apos;,&apos;pseudo-reverse&apos;,&apos;reverse&apos;,&apos;shift&apos;)' required='false' advanced='false' restrictions='shuffle,pseudo-reverse,reverse,shift' />
      <ITEM name='decoy_tag' value='DECOY_' type='string' description='decoy tag' required='false' advanced='false' />
      <ITEM name='min_decoy_fraction' value='0.8' type='double' description='Minimum fraction of decoy / target peptides and proteins' required='false' advanced='true' />
      <ITEM name='aim_decoy_fraction' value='1.0' type='double' description='Number of decoys the algorithm should generate (if unequal to 1, the algorithm will randomly select N peptides for decoy generation)' required='false' advanced='true' />
      <ITEM name='shuffle_max_attempts' value='30' type='int' description='shuffle: maximum attempts to lower the amino acid sequence identity between target and decoy for the shuffle algorithm' required='false' advanced='true' />
      <ITEM name='shuffle_sequence_identity_threshold' value='0.5' type='double' description='shuffle: target-decoy amino acid sequence identity threshold for the shuffle algorithm' required='false' advanced='true' />
      <ITEM name='shift_precursor_mz_shift' value='0.0' type='double' description='shift: precursor ion MZ shift in Thomson for shift decoy method' required='false' advanced='true' />
      <ITEM name='shift_product_mz_shift' value='20.0' type='double' description='shift: fragment ion MZ shift in Thomson for shift decoy method' required='false' advanced='true' />
      <ITEM name='product_mz_threshold' value='0.025' type='double' description='MZ threshold in Thomson for fragment ion annotation' required='false' advanced='true' />
      <ITEM name='allowed_fragment_types' value='b,y' type='string' description='allowed fragment types' required='false' advanced='true' />
      <ITEM name='allowed_fragment_charges' value='1,2,3,4' type='string' description='allowed fragment charge states' required='false' advanced='true' />
      <ITEM name='enable_detection_specific_losses' value='false' type='bool' description='set this flag if specific neutral losses for detection fragment ions should be allowed' required='false' advanced='true' />
      <ITEM name='enable_detection_unspecific_losses' value='false' type='bool' description='set this flag if unspecific neutral losses (H2O1, H3N1, C1H2N2, C1H2N1O1) for detection fragment ions should be allowed' required='false' advanced='true' />
      <ITEM name='switchKR' value='true' type='string' description='Whether to switch terminal K and R (to achieve different precursor mass)' required='false' advanced='false' restrictions='true,false' />
      <ITEM name='separate' value='false' type='bool' description='set this flag if decoys should not be appended to targets.' required='false' advanced='true' />
      <ITEM name='log' value='' type='string' description='Name of log file (created only when specified)' required='false' advanced='true' />
      <ITEM name='debug' value='0' type='int' description='Sets the debug level' required='false' advanced='true' />
      <ITEM name='threads' value='1' type='int' description='Sets the number of threads allowed to be used by the TOPP tool' required='false' advanced='false' />
      <ITEM name='no_progress' value='false' type='bool' description='Disables progress logging to command line' required='false' advanced='true' />
      <ITEM name='force' value='false' type='bool' description='Overwrite tool specific checks.' required='false' advanced='true' />
      <ITEM name='test' value='false' type='bool' description='Enables the test mode (needed for internal use only)' required='false' advanced='true' />
    </NODE>
  </NODE>
  
  
  <NODE name='OpenSwathWorkflow' description='Complete workflow to run OpenSWATH'>
    <NODE name='1' description='Instance &apos;1&apos; section for &apos;OpenSwathWorkflow&apos;'>     
      <ITEM name='tr_irt_nonlinear' value='' type='input-file' description='additional nonlinear transition file (&apos;TraML&apos;)' required='false' advanced='false' supported_formats='*.traML,*.tsv,*.pqp' />
      <ITEM name='rt_norm' value='' type='input-file' description='RT normalization file (how to map the RTs of this run to the ones stored in the library). If set, tr_irt may be omitted.' required='false' advanced='true' supported_formats='*.trafoXML' />
      <ITEM name='swath_windows_file' value='' type='input-file' description='Optional, tab-separated file containing the SWATH windows for extraction: lower_offset upper_offset. Note that the first line is a header and will be skipped.' required='false' advanced='true' />
      <ITEM name='sort_swath_maps' value='false' type='bool' description='Sort input SWATH files when matching to SWATH windows from swath_windows_file' required='false' advanced='true' />
      <ITEM name='out_features' value='' type='output-file' description='output file' required='false' advanced='false' supported_formats='*.featureXML' />
      <ITEM name='out_chrom' value='' type='output-file' description='Also output all computed chromatograms output in mzML (chrom.mzML) or sqMass (SQLite format)' required='false' advanced='true' supported_formats='*.mzML,*.sqMass' />
      <ITEM name='out_qc' value='' type='output-file' description='Optional QC meta data (charge distribution in MS1). Only works with mzML input files.' required='false' advanced='true' supported_formats='*.json' />
      <ITEM name='min_upper_edge_dist' value='0.0' type='double' description='Minimal distance to the upper edge of a Swath window to still consider a precursor, in Thomson' required='false' advanced='true' />
      <ITEM name='sonar' value='false' type='bool' description='data is scanning SWATH data' required='false' advanced='false' />
      <ITEM name='rt_extraction_window' value='600.0' type='double' description='Only extract RT around this value (-1 means extract over the whole range, a value of 600 means to extract around +/- 300 s of the expected elution).' required='false' advanced='false' />
      <ITEM name='extra_rt_extraction_window' value='0.0' type='double' description='Output an XIC with a RT-window by this much larger (e.g. to visually inspect a larger area of the chromatogram)' required='false' advanced='true' restrictions='0.0:' />
      <ITEM name='mz_extraction_window' value='0.05' type='double' description='Extraction window in Thomson or ppm (see mz_extraction_window_unit)' required='false' advanced='false' restrictions='0.0:' />
      <ITEM name='mz_extraction_window_unit' value='Th' type='string' description='Unit for mz extraction' required='false' advanced='true' restrictions='Th,ppm' />
      <ITEM name='mz_extraction_window_ms1' value='0.05' type='double' description='Extraction window used in MS1 in Thomson or ppm (see mz_extraction_window_ms1_unit)' required='false' advanced='false' restrictions='0.0:' />
      <ITEM name='mz_extraction_window_ms1_unit' value='Th' type='string' description='Unit of the MS1 m/z extraction window' required='false' advanced='true' restrictions='ppm,Th' />
      <ITEM name='use_ms1_ion_mobility' value='true' type='string' description='Also perform precursor extraction using the same ion mobility window as for fragment ion extraction' required='false' advanced='true' restrictions='true,false' />
      <ITEM name='matching_window_only' value='false' type='bool' description='Assume the input data is targeted / PRM-like data with potentially overlapping DIA windows. Will only attempt to extract each assay from the *best* matching DIA window (instead of all matching windows).' required='false' advanced='true' />
      <ITEM name='irt_mz_extraction_window' value='0.05' type='double' description='Extraction window used for iRT and m/z correction in Thomson or ppm (see irt_mz_extraction_window_unit)' required='false' advanced='true' restrictions='0.0:' />
      <ITEM name='irt_mz_extraction_window_unit' value='Th' type='string' description='Unit for mz extraction' required='false' advanced='true' restrictions='Th,ppm' />
      <ITEM name='irt_im_extraction_window' value='-1.0' type='double' description='Ion mobility extraction window used for iRT (in 1/K0 or milliseconds)' required='false' advanced='true' />
      <ITEM name='min_rsq' value='0.95' type='double' description='Minimum r-squared of RT peptides regression' required='false' advanced='true' />
      <ITEM name='min_coverage' value='0.6' type='double' description='Minimum relative amount of RT peptides to keep' required='false' advanced='true' />
      <ITEM name='split_file_input' value='false' type='bool' description='The input files each contain one single SWATH (alternatively: all SWATH are in separate files)' required='false' advanced='true' />
      <ITEM name='use_elution_model_score' value='false' type='bool' description='Turn on elution model score (EMG fit to peak)' required='false' advanced='true' />
      <ITEM name='readOptions' value='normal' type='string' description='Whether to run OpenSWATH directly on the input data, cache data to disk first or to perform a datareduction step first. If you choose cache, make sure to also set tempDirectory' required='false' advanced='true' restrictions='normal,cache,cacheWorkingInMemory,workingInMemory' />
      <ITEM name='mz_correction_function' value='none' type='string' description='Use the retention time normalization peptide MS2 masses to perform a mass correction (linear, weighted by intensity linear or quadratic) of all spectra.' required='false' advanced='true' restrictions='none,regression_delta_ppm,unweighted_regression,weighted_regression,quadratic_regression,weighted_quadratic_regression,weighted_quadratic_regression_delta_ppm,quadratic_regression_delta_ppm' />
      <ITEM name='tempDirectory' value='C:/Users/root/AppData/Local/Temp' type='string' description='Temporary directory to store cached files for example' required='false' advanced='true' />
      <ITEM name='extraction_function' value='tophat' type='string' description='Function used to extract the signal' required='false' advanced='true' restrictions='tophat,bartlett' />
      <ITEM name='batchSize' value='250' type='int' description='The batch size of chromatograms to process (0 means to only have one batch, sensible values are around 250-1000)' required='false' advanced='true' restrictions='0:' />
      <ITEM name='outer_loop_threads' value='-1' type='int' description='How many threads should be used for the outer loop (-1 use all threads, use 4 to analyze 4 SWATH windows in memory at once).' required='false' advanced='true' />
      <ITEM name='ms1_isotopes' value='0' type='int' description='The number of MS1 isotopes used for extraction' required='false' advanced='true' restrictions='0:' />
      <ITEM name='log' value='' type='string' description='Name of log file (created only when specified)' required='false' advanced='true' />
      <ITEM name='debug' value='0' type='int' description='Sets the debug level' required='false' advanced='true' />
      <ITEM name='threads' value='1' type='int' description='Sets the number of threads allowed to be used by the TOPP tool' required='false' advanced='false' />
      <ITEM name='no_progress' value='false' type='bool' description='Disables progress logging to command line' required='false' advanced='true' />
      <ITEM name='force' value='false' type='bool' description='Overwrite tool specific checks.' required='false' advanced='true' />
      <ITEM name='test' value='false' type='bool' description='Enables the test mode (needed for internal use only)' required='false' advanced='true' />
      <NODE name='Debugging' description='Debugging'>
        <ITEM name='irt_mzml' value='' type='output-file' description='Chromatogram mzML containing the iRT peptides' required='false' advanced='false' supported_formats='*.mzML' />
        <ITEM name='irt_trafo' value='' type='output-file' description='Transformation file for RT transform' required='false' advanced='false' supported_formats='*.trafoXML' />
      </NODE>
      <NODE name='Calibration' description='Parameters for the m/z and ion mobility calibration.'>
        <ITEM name='ms1_im_calibration' value='false' type='bool' description='Whether to use MS1 precursor data for the ion mobility calibration (default = false, uses MS2 / fragment ions for calibration)' required='false' advanced='true' />
        <ITEM name='im_correction_function' value='linear' type='string' description='Type of normalization function for IM calibration.' required='false' advanced='false' restrictions='none,linear' />
        <ITEM name='debug_im_file' value='' type='string' description='Debug file for Ion Mobility calibration.' required='false' advanced='false' />
        <ITEM name='debug_mz_file' value='' type='string' description='Debug file for m/z calibration.' required='false' advanced='false' />
      </NODE>
      <NODE name='Library' description='Library parameters section'>
        <ITEM name='retentionTimeInterpretation' value='iRT' type='string' description='How to interpret the provided retention time (the retention time column can either be interpreted to be in iRT, minutes or seconds)' required='false' advanced='true' restrictions='iRT,seconds,minutes' />
        <ITEM name='override_group_label_check' value='false' type='bool' description='Override an internal check that assures that all members of the same PeptideGroupLabel have the same PeptideSequence (this ensures that only different isotopic forms of the same peptide can be grouped together in the same label group). Only turn this off if you know what you are doing.' required='false' advanced='true' />
        <ITEM name='force_invalid_mods' value='false' type='bool' description='Force reading even if invalid modifications are encountered (OpenMS may not recognize the modification)' required='false' advanced='true' />
      </NODE>
      <NODE name='RTNormalization' description='Parameters for the RTNormalization for iRT petides. This specifies how the RT alignment is performed and how outlier detection is applied. Outlier detection can be done iteratively (by default) which removes one outlier per iteration or using the RANSAC algorithm.'>
        <ITEM name='alignmentMethod' value='linear' type='string' description='How to perform the alignment to the normalized RT space using anchor points. &apos;linear&apos;: perform linear regression (for few anchor points). &apos;interpolated&apos;: Interpolate between anchor points (for few, noise-free anchor points). &apos;lowess&apos; Use local regression (for many, noisy anchor points). &apos;b_spline&apos; use b splines for smoothing.' required='false' advanced='false' restrictions='linear,interpolated,lowess,b_spline' />
        <ITEM name='outlierMethod' value='iter_residual' type='string' description='Which outlier detection method to use (valid: &apos;iter_residual&apos;, &apos;iter_jackknife&apos;, &apos;ransac&apos;, &apos;none&apos;). Iterative methods remove one outlier at a time. Jackknife approach optimizes for maximum r-squared improvement while &apos;iter_residual&apos; removes the datapoint with the largest residual error (removal by residual is computationally cheaper, use this with lots of peptides).' required='false' advanced='false' restrictions='iter_residual,iter_jackknife,ransac,none' />
        <ITEM name='useIterativeChauvenet' value='false' type='bool' description='Whether to use Chauvenet&apos;s criterion when using iterative methods. This should be used if the algorithm removes too many datapoints but it may lead to true outliers being retained.' required='false' advanced='false' />
        <ITEM name='RANSACMaxIterations' value='1000' type='int' description='Maximum iterations for the RANSAC outlier detection algorithm.' required='false' advanced='false' />
        <ITEM name='RANSACMaxPercentRTThreshold' value='3' type='int' description='Maximum threshold in RT dimension for the RANSAC outlier detection algorithm (in percent of the total gradient). Default is set to 3% which is around +/- 4 minutes on a 120 gradient.' required='false' advanced='false' />
        <ITEM name='RANSACSamplingSize' value='10' type='int' description='Sampling size of data points per iteration for the RANSAC outlier detection algorithm.' required='false' advanced='false' />
        <ITEM name='estimateBestPeptides' value='false' type='bool' description='Whether the algorithms should try to choose the best peptides based on their peak shape for normalization. Use this option you do not expect all your peptides to be detected in a sample and too many &apos;bad&apos; peptides enter the outlier removal step (e.g. due to them being endogenous peptides or using a less curated list of peptides).' required='false' advanced='false' />
        <ITEM name='InitialQualityCutoff' value='0.5' type='double' description='The initial overall quality cutoff for a peak to be scored (range ca. -2 to 2)' required='false' advanced='false' />
        <ITEM name='OverallQualityCutoff' value='5.5' type='double' description='The overall quality cutoff for a peak to go into the retention time estimation (range ca. 0 to 10)' required='false' advanced='false' />
        <ITEM name='NrRTBins' value='10' type='int' description='Number of RT bins to use to compute coverage. This option should be used to ensure that there is a complete coverage of the RT space (this should detect cases where only a part of the RT gradient is actually covered by normalization peptides)' required='false' advanced='false' />
        <ITEM name='MinPeptidesPerBin' value='1' type='int' description='Minimal number of peptides that are required for a bin to counted as &apos;covered&apos;' required='false' advanced='false' />
        <ITEM name='MinBinsFilled' value='8' type='int' description='Minimal number of bins required to be covered' required='false' advanced='false' />
        <NODE name='lowess' description=''>
          <ITEM name='span' value='0.666666666666667' type='double' description='Span parameter for lowess' required='false' advanced='false' restrictions='0.0:1.0' />
        </NODE>
        <NODE name='b_spline' description=''>
          <ITEM name='num_nodes' value='5' type='int' description='Number of nodes for b spline' required='false' advanced='false' restrictions='0:' />
        </NODE>
      </NODE>
      <NODE name='Scoring' description='Scoring parameters section'>
        <ITEM name='stop_report_after_feature' value='5' type='int' description='Stop reporting after feature (ordered by quality; -1 means do not stop).' required='false' advanced='false' />
        <ITEM name='rt_normalization_factor' value='100.0' type='double' description='The normalized RT is expected to be between 0 and 1. If your normalized RT has a different range, pass this here (e.g. it goes from 0 to 100, set this value to 100)' required='false' advanced='false' />
        <ITEM name='quantification_cutoff' value='0.0' type='double' description='Cutoff in m/z below which peaks should not be used for quantification any more' required='false' advanced='true' restrictions='0.0:' />
        <ITEM name='write_convex_hull' value='false' type='bool' description='Whether to write out all points of all features into the featureXML' required='false' advanced='true' />
        <ITEM name='spectrum_addition_method' value='simple' type='string' description='For spectrum addition, either use simple concatenation or use peak resampling' required='false' advanced='true' restrictions='simple,resample' />
        <ITEM name='add_up_spectra' value='1' type='int' description='Add up spectra around the peak apex (needs to be a non-even integer)' required='false' advanced='true' restrictions='1:' />
        <ITEM name='spacing_for_spectra_resampling' value='0.005' type='double' description='If spectra are to be added, use this spacing to add them up' required='false' advanced='true' restrictions='0.0:' />
        <ITEM name='uis_threshold_sn' value='0' type='int' description='S/N threshold to consider identification transition (set to -1 to consider all)' required='false' advanced='false' />
        <ITEM name='uis_threshold_peak_area' value='0' type='int' description='Peak area threshold to consider identification transition (set to -1 to consider all)' required='false' advanced='false' />
        <ITEM name='scoring_model' value='default' type='string' description='Scoring model to use' required='false' advanced='true' restrictions='default,single_transition' />
        <ITEM name='im_extra_drift' value='0.0' type='double' description='Extra drift time to extract for IM scoring (as a fraction, e.g. 0.25 means 25% extra on each side)' required='false' advanced='true' restrictions='0.0:' />
        <NODE name='TransitionGroupPicker' description=''>
          <ITEM name='stop_after_feature' value='-1' type='int' description='Stop finding after feature (ordered by intensity; -1 means do not stop).' required='false' advanced='false' />
          <ITEM name='min_peak_width' value='-1.0' type='double' description='Minimal peak width (s), discard all peaks below this value (-1 means no action).' required='false' advanced='false' />
          <ITEM name='peak_integration' value='original' type='string' description='Calculate the peak area and height either the smoothed or the raw chromatogram data.' required='false' advanced='true' restrictions='original,smoothed' />
          <ITEM name='background_subtraction' value='none' type='string' description='Remove background from peak signal using estimated noise levels. The &apos;original&apos; method is only provided for historical purposes, please use the &apos;exact&apos; method and set parameters using the PeakIntegrator: settings. The same original or smoothed chromatogram specified by peak_integration will be used for background estimation.' required='false' advanced='false' restrictions='none,original,exact' />
          <ITEM name='recalculate_peaks' value='true' type='string' description='Tries to get better peak picking by looking at peak consistency of all picked peaks. Tries to use the consensus (median) peak border if the variation within the picked peaks is too large.' required='false' advanced='false' restrictions='true,false' />
          <ITEM name='use_precursors' value='false' type='bool' description='Use precursor chromatogram for peak picking (note that this may lead to precursor signal driving the peak picking)' required='false' advanced='true' />
          <ITEM name='use_consensus' value='true' type='string' description='Use consensus peak boundaries when computing transition group picking (if false, compute independent peak boundaries for each transition)' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='recalculate_peaks_max_z' value='0.75' type='double' description='Determines the maximal Z-Score (difference measured in standard deviations) that is considered too large for peak boundaries. If the Z-Score is above this value, the median is used for peak boundaries (default value 1.0).' required='false' advanced='false' />
          <ITEM name='minimal_quality' value='-1.5' type='double' description='Only if compute_peak_quality is set, this parameter will not consider peaks below this quality threshold' required='false' advanced='false' />
          <ITEM name='resample_boundary' value='15.0' type='double' description='For computing peak quality, how many extra seconds should be sample left and right of the actual peak' required='false' advanced='true' />
          <ITEM name='compute_peak_quality' value='true' type='string' description='Tries to compute a quality value for each peakgroup and detect outlier transitions. The resulting score is centered around zero and values above 0 are generally good and below -1 or -2 are usually bad.' required='false' advanced='false' restrictions='true,false' />
          <ITEM name='compute_peak_shape_metrics' value='false' type='bool' description='Calculates various peak shape metrics (e.g., tailing) that can be used for downstream QC/QA.' required='false' advanced='true' />
          <ITEM name='compute_total_mi' value='false' type='bool' description='Compute mutual information metrics for individual transitions that can be used for OpenSWATH/IPF scoring.' required='false' advanced='true' />
          <ITEM name='boundary_selection_method' value='largest' type='string' description='Method to use when selecting the best boundaries for peaks.' required='false' advanced='true' restrictions='largest,widest' />
          <NODE name='PeakPickerMRM' description=''>
            <ITEM name='sgolay_frame_length' value='11' type='int' description='The number of subsequent data points used for smoothing.#br#This number has to be uneven. If it is not, 1 will be added.' required='false' advanced='false' />
            <ITEM name='sgolay_polynomial_order' value='3' type='int' description='Order of the polynomial that is fitted.' required='false' advanced='false' />
            <ITEM name='gauss_width' value='30.0' type='double' description='Gaussian width in seconds, estimated peak size.' required='false' advanced='false' />
            <ITEM name='use_gauss' value='false' type='string' description='Use Gaussian filter for smoothing (alternative is Savitzky-Golay filter)' required='false' advanced='false' restrictions='false,true' />
            <ITEM name='peak_width' value='-1.0' type='double' description='Force a certain minimal peak_width on the data (e.g. extend the peak at least by this amount on both sides) in seconds. -1 turns this feature off.' required='false' advanced='false' />
            <ITEM name='signal_to_noise' value='0.1' type='double' description='Signal-to-noise threshold at which a peak will not be extended any more. Note that setting this too high (e.g. 1.0) can lead to peaks whose flanks are not fully captured.' required='false' advanced='false' restrictions='0.0:' />
            <ITEM name='write_sn_log_messages' value='false' type='bool' description='Write out log messages of the signal-to-noise estimator in case of sparse windows or median in rightmost histogram bin' required='false' advanced='false' />
            <ITEM name='remove_overlapping_peaks' value='true' type='string' description='Try to remove overlapping peaks during peak picking' required='false' advanced='false' restrictions='false,true' />
            <ITEM name='method' value='corrected' type='string' description='Which method to choose for chromatographic peak-picking (OpenSWATH legacy on raw data, corrected picking on smoothed chromatogram or Crawdad on smoothed chromatogram).' required='false' advanced='false' restrictions='legacy,corrected,crawdad' />
          </NODE>
          <NODE name='PeakIntegrator' description=''>
            <ITEM name='integration_type' value='intensity_sum' type='string' description='The integration technique to use in integratePeak() and estimateBackground() which uses either the summed intensity, integration by Simpson&apos;s rule or trapezoidal integration.' required='false' advanced='false' restrictions='intensity_sum,simpson,trapezoid' />
            <ITEM name='baseline_type' value='base_to_base' type='string' description='The baseline type to use in estimateBackground() based on the peak boundaries. A rectangular baseline shape is computed based either on the minimal intensity of the peak boundaries, the maximum intensity or the average intensity (base_to_base).' required='false' advanced='false' restrictions='base_to_base,vertical_division,vertical_division_min,vertical_division_max' />
            <ITEM name='fit_EMG' value='false' type='string' description='Fit the chromatogram/spectrum to the EMG peak model.' required='false' advanced='false' restrictions='false,true' />
          </NODE>
        </NODE>
        <NODE name='DIAScoring' description=''>
          <ITEM name='dia_extraction_window' value='0.05' type='double' description='DIA extraction window in Th or ppm.' required='false' advanced='false' restrictions='0.0:' />
          <ITEM name='dia_extraction_unit' value='Th' type='string' description='DIA extraction window unit' required='false' advanced='false' restrictions='Th,ppm' />
          <ITEM name='dia_centroided' value='false' type='bool' description='Use centroided DIA data.' required='false' advanced='false' />
          <ITEM name='dia_byseries_intensity_min' value='300.0' type='double' description='DIA b/y series minimum intensity to consider.' required='false' advanced='false' restrictions='0.0:' />
          <ITEM name='dia_byseries_ppm_diff' value='10.0' type='double' description='DIA b/y series minimal difference in ppm to consider.' required='false' advanced='false' restrictions='0.0:' />
          <ITEM name='dia_nr_isotopes' value='4' type='int' description='DIA number of isotopes to consider.' required='false' advanced='false' restrictions='0:' />
          <ITEM name='dia_nr_charges' value='4' type='int' description='DIA number of charges to consider.' required='false' advanced='false' restrictions='0:' />
          <ITEM name='peak_before_mono_max_ppm_diff' value='20.0' type='double' description='DIA maximal difference in ppm to count a peak at lower m/z when searching for evidence that a peak might not be monoisotopic.' required='false' advanced='false' restrictions='0.0:' />
        </NODE>
        <NODE name='EMGScoring' description=''>
          <ITEM name='max_iteration' value='10' type='int' description='Maximum number of iterations using by Levenberg-Marquardt algorithm.' required='false' advanced='false' />
        </NODE>
        <NODE name='Scores' description=''>
          <ITEM name='use_shape_score' value='true' type='string' description='Use the shape score (this score measures the similarity in shape of the transitions using a cross-correlation)' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_coelution_score' value='true' type='string' description='Use the coelution score (this score measures the similarity in coelution of the transitions using a cross-correlation)' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_rt_score' value='true' type='string' description='Use the retention time score (this score measure the difference in retention time)' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_library_score' value='true' type='string' description='Use the library score' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_intensity_score' value='true' type='string' description='Use the intensity score' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_nr_peaks_score' value='true' type='string' description='Use the number of peaks score' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_total_xic_score' value='true' type='string' description='Use the total XIC score' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_total_mi_score' value='false' type='bool' description='Use the total MI score' required='false' advanced='true' />
          <ITEM name='use_sn_score' value='true' type='string' description='Use the SN (signal to noise) score' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_dia_scores' value='true' type='string' description='Use the DIA (SWATH) scores. If turned off, will not use fragment ion spectra for scoring.' required='false' advanced='true' restrictions='true,false' />
          <ITEM name='use_ms1_correlation' value='false' type='bool' description='Use the correlation scores with the MS1 elution profiles' required='false' advanced='true' />
          <ITEM name='use_sonar_scores' value='false' type='bool' description='Use the scores for SONAR scans (scanning swath)' required='false' advanced='true' />
          <ITEM name='use_ms1_fullscan' value='false' type='bool' description='Use the full MS1 scan at the peak apex for scoring (ppm accuracy of precursor and isotopic pattern)' required='false' advanced='true' />
          <ITEM name='use_ms1_mi' value='false' type='bool' description='Use the MS1 MI score' required='false' advanced='true' />
          <ITEM name='use_uis_scores' value='false' type='bool' description='Use UIS scores for peptidoform identification ' required='false' advanced='true' />
        </NODE>
      </NODE>
    </NODE>
  </NODE>

  
</PARAMETERS>
";

        string run_ini = "";

        private void button2_Click(object sender, EventArgs e)
        {

            fsave.Filter = "Preset|*.cfg";
            if (fsave.ShowDialog() == DialogResult.OK)
            {
                wd = fsave.FileName.Substring(0, fsave.FileName.LastIndexOf('\\') + 1);
                StreamWriter sw = new StreamWriter(fsave.FileName);
                StringReader sr = new StringReader(this.Tcomment.Text);
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    sw.Write('#');
                    sw.WriteLine(line);
                }
                sr.Close();
                sw.Write("irt.path=");
                sw.WriteLine(Tirt.Text);
                sw.Write("py.path=");
                sw.WriteLine(Tpy.Text);
                sw.Write("lib.path=");
                sw.WriteLine(Tlib.Text);
                sw.Write("db.path=");
                sw.WriteLine(Tdb.Text);
                sw.Write("win.path=");
                sw.WriteLine(Twin.Text);
                sw.Write("mod.path=");
                sw.WriteLine(Tmod.Text);
                sw.Write("ipf.enable=");
                if (Cipf.Checked)
                {
                    sw.WriteLine(1);
                }
                else
                {
                    sw.WriteLine(0);
                }
                sw.Write("im.use=");
                if (Cim.Checked)
                {
                    sw.WriteLine(1);
                }
                else
                {
                    sw.WriteLine(0);
                }

                sw.Write("im.win=");
                sw.WriteLine(Nim.Value);

                for (int i = 0; i < Lin.Items.Count; ++i)
                {
                    sw.Write("xml-input-list+=");
                    sw.WriteLine(Lin.Items[i].ToString());
                }

                sr = new StringReader(this.run_ini);

                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    sw.WriteLine("|" + line);
                }

                sr.Close();

                sw.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            fopen.Multiselect = false;
            fopen.Filter = "Preset|*.cfg";
            if (fopen.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(fopen.FileName);
                this.Tcomment.Text = "";
                this.Tdb.Text = "";
                this.Tlib.Text = "";
                this.Tmod.Text = "";
                this.Tirt.Text = "";
                this.Tpy.Text = "";
                this.Twin.Text = "";
                this.Lin.Items.Clear();
                this.Cipf.Checked = false;
                this.Cim.Checked = false;
                this.run_ini = "";
                this.Nim.Value = 0.04M;
                char[] sp = { '=' };
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith("|"))
                    {
                        run_ini += line.Substring(1) + System.Environment.NewLine;
                    }
                    if (line.StartsWith("#"))
                    {
                        Tcomment.Text += line.Substring(1) + System.Environment.NewLine;
                        continue;
                    }
                    if (line.StartsWith("irt.path="))
                    {
                        Tirt.Text = line.Split(sp, 2)[1];
                        continue;
                    }
                    if (line.StartsWith("py.path="))
                    {
                        Tpy.Text = line.Split(sp, 2)[1];
                        continue;
                    }
                    if (line.StartsWith("lib.path="))
                    {
                        Tlib.Text = line.Split(sp, 2)[1];
                        continue;
                    }
                    if (line.StartsWith("db.path="))
                    {
                        Tdb.Text = line.Split(sp, 2)[1];
                        continue;
                    }
                    if (line.StartsWith("mod.path="))
                    {
                        Tmod.Text = line.Split(sp, 2)[1];
                        continue;
                    }
                    if (line.StartsWith("win.path="))
                    {
                        Twin.Text = line.Split(sp, 2)[1];
                        continue;
                    }
                    if (line.StartsWith("ipf.enable="))
                    {
                        Cipf.Checked = line.Split(sp, 2)[1].Equals("1");
                    }
                    if (line.StartsWith("im.use="))
                    {
                        Cim.Checked = line.Split(sp, 2)[1].Equals("1");
                    }
                    if (line.StartsWith("im.win="))
                    {
                        Nim.Value = Decimal.Parse(line.Split(sp, 2)[1]);
                    }
                    if (line.StartsWith("xml-input-list+="))
                    {
                        Lin.Items.Add(line.Split(sp, 2)[1]);
                        continue;
                    }
                }
                if (run_ini.Length == 0)
                {
                    run_ini = run_ini_template;
                }
                sr.Close();
            }
        }

        string[] select_file(string filter, bool dir, bool multi = false)
        {
            string[] ret = new string[] { "" };
            fopen.Multiselect = multi;
            fopen.Filter = filter;
            if (fopen.ShowDialog() == DialogResult.OK)
            {
                if (!multi)
                {
                    if (dir)
                    {
                        ret[0] = fopen.FileName.Substring(0, 1 + fopen.FileName.LastIndexOf('\\'));
                    }
                    else
                    {
                        ret[0] = fopen.FileName;
                    }
                    return ret;
                }
                else
                {
                    if (dir)
                    {
                        List<string> tmp = new List<string>();
                        for (int i = 0; i < fopen.FileNames.Length; ++i)
                        {
                            string str = fopen.FileNames[i];
                            tmp.Add(str.Substring(0, 1 + str.LastIndexOf('\\')));
                        }
                        return tmp.Distinct().ToArray();
                    }
                    else
                    {
                        return fopen.FileNames;
                    }
                }
            }
            return ret;
        }
        private TextBox path_Click(object sender, string fn, bool dir)
        {
            TextBox o = (TextBox)sender;
            o.Text = select_file(fn, dir)[0];
            return o;
        }
        private void Tirt_Click(object sender, EventArgs e)
        {
            path_Click(sender, "iRT|*.txt", false);
        }

        private void Tpy_Click(object sender, EventArgs e)
        {
            path_Click(sender, "Python3|python.exe", true);
        }

        private void Tlib_Click(object sender, EventArgs e)
        {
            path_Click(sender, "Library|*.blib;*.sptxt;*.tsv", false);
        }

        private void Tdb_Click(object sender, EventArgs e)
        {
            path_Click(sender, "Database|*.fasta", false);
        }

        private void Tmod_Click(object sender, EventArgs e)
        {
            path_Click(sender, "UniMod|*.xml", false);
        }

        private void Twin_Click(object sender, EventArgs e)
        {
            path_Click(sender, "SWATH window|*.txt", false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] add = select_file("Data|*.mzXML;*.mzML", false, true);
            for (int i = 0; i < add.Length; ++i)
            {
                if (Lin.Items.Contains(add[i]))
                {
                    continue;
                }
                Lin.Items.Add(add[i]);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<int> t = new List<int>();
            for (int idx = Lin.Items.Count - 1; idx >= 0; --idx)
            {
                if (Lin.SelectedIndices.Contains(idx))
                {
                    t.Add(idx);
                }
            }
            for (int i = 0; i < t.Count; ++i)
            {
                Lin.Items.RemoveAt(t[i]);
            }
            Lin.ClearSelected();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Lin.ClearSelected();
            Lin.Items.Clear();
        }

        List<string[]> cmd = new List<string[]>();

        void setupIni()
        {
            StreamWriter sw = new StreamWriter(wd + "\\run.ini");
            sw.WriteLine(run_ini);
            sw.Close();
            Process p = new Process();
            p.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            p.StartInfo.FileName = "INIFileEditor.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = wd+"\\run.ini";
            p.Start();
            p.WaitForExit();
        }
        private void button7_Click(object sender, EventArgs e)
        {

            int core = Environment.ProcessorCount;
            cmd = new List<string[]>();

            string[] tmp;
            string str = "";

            if (!File.Exists(Tlib.Text) || !File.Exists(Tirt.Text) || !File.Exists(Tdb.Text) || !Directory.Exists(Tpy.Text) || !File.Exists(Twin.Text) || Lin.Items.Count == 0)
            {
                MessageBox.Show("empty path or path not valid");
                return;
            }
            for (int i = 0; i < Lin.Items.Count; ++i)
            {
                if (!File.Exists(Lin.Items[i].ToString()))
                {
                    MessageBox.Show("some input xml files not really exist");
                    return;
                }
            }
            if (Cipf.Checked && Tmod.Text.Length == 0)
            {
                MessageBox.Show("ipf need subset of unimod.xml");
                return;
            }


            if (wd == null)
            {
                button2_Click(null, null);
            }
            if (wd == null)
            {
                return;
            }

            if (run_ini.Length == 0)
            {
                run_ini = run_ini_template;
            }
            setupIni();

            /*/
            if (!Tlib.Text.EndsWith("tsv"))
            {
                tmp = new string[1];
                if (Tmod.Text.Length > 0)
                {
                    tmp[0] = String.Format("Lib2Tsv.exe -in {1} -out {0}{2} -db {3} -unimod_file {4} -threads {5}", wd, Tlib.Text, "lib.tsv", Tdb.Text, Tmod.Text, core);
                }
                else
                {
                    tmp[0] = String.Format("Lib2Tsv.exe -in {1} -out {0}{2} -db {3} -threads {4}", wd, Tlib.Text, "lib.tsv", Tdb.Text, core);
                }
                cmd.Add(tmp);

                tmp = new string[] { String.Format("OpenSwathAssayGenerator.exe -in {0}lib.tsv -out {0}lib.os.tsv -product_lower_mz_limit 100 -precursor_mz_threshold 0.1 -product_mz_threshold 0.05 -swath_windows_file {1}", wd, Twin.Text) };
                cmd.Add(tmp);
            }
            else
            {
                //tmp = new string[] { String.Format("copy {0} {1}lib.os.tsv" ,Tlib.Text,wd)};
                //cmd.Add(tmp);
                cmd.Add(new string[] { ""});
                cmd.Add(new string[] { ""});

            }
            /*/

            str = "";
            if (Tmod.Text.Length > 0)
            {
                str = " -unimod_file " + Tmod.Text;
            }

            if (Tlib.Text.EndsWith("tsv") || Tlib.Text.EndsWith("csv"))
            {
                ;
            }
            else
            {
                tmp = new string[] { String.Format("Lib2Tsv.exe -in {1} -out {0}{2} -db {3} -threads {4}{5}", wd, Tlib.Text, "lib.tsv", Tdb.Text, core, str) };
                cmd.Add(tmp);

                if (Cipf.Checked)
                {
                    str += " -enable_ipf";
                }
                tmp = new string[] { String.Format("OpenSwathAssayGenerator.exe -ini {0}run.ini -in {0}lib.tsv -out {0}lib.os.tsv -swath_windows_file {1}{2}", wd, Twin.Text, str) };
                cmd.Add(tmp);
            }

            tmp = new string[] { String.Format("OpenSwathAssayGenerator.exe -ini {0}run.ini -in {0}irt.tsv -out {0}irt.TraML -swath_windows_file {1}", wd, Twin.Text) };
            cmd.Add(tmp);

            tmp = new string[] { String.Format("OpenSwathDecoyGenerator.exe -ini {0}run.ini -in {0}lib.os.tsv -out {0}lib.os.pqp", wd) };
            cmd.Add(tmp);

            for (int i = 0; i < Lin.Items.Count; ++i)
            {
                str = Lin.Items[i].ToString();
                str = str.Substring(1 + str.LastIndexOf('\\'));
                string t = "";
                if (Cipf.Checked)
                {
                    t += " -enable_uis_scoring -Scoring:Scores:use_mi_score ";//-Scoring:TransitionGroupPicker:compute_total_mi 
                }
                if (Cim.Checked)
                {
                    t += String.Format(" -ion_mobility_window {0} -im_extraction_window_ms1 {0} -irt_im_extraction_window {0} -Scoring:Scores:use_ion_mobility_scores ", Nim.Value);
                }
                tmp = new string[] { String.Format("OpenSwathWorkflow.exe -ini {0}run.ini -use_ms1_traces {5} -tr_irt {0}irt.TraML -tr {0}lib.os.pqp -threads {1} -swath_windows_file {4} -out_osw {0}{2}.osw -in {3}", wd, core, str, Lin.Items[i].ToString(), Twin.Text, t) };
                cmd.Add(tmp);
            }

            if (Cipf.Checked)
            {


                str = "";
                for (int i = 0; i < Lin.Items.Count; ++i)
                {
                    string t = Lin.Items[i].ToString();
                    t = t.Substring(1 + t.LastIndexOf('\\'));
                    str += String.Format("{0}{1}.osw ", wd, t);
                }

                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe merge --template={1}lib.os.pqp --out={1}model.oswm {2}", Tpy.Text, wd, str) };
                cmd.Add(tmp);


                /*/
                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe score --in={1}model.oswm --level=ms1 --threads {2:D}", Tpy.Text, wd, core) };
                cmd.Add(tmp);
                /*/


                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe score --in={1}model.oswm --level=ms2 --threads {2:D}", Tpy.Text, wd, core) };
                cmd.Add(tmp);

                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe score --in={1}model.oswm --level=transition --threads {2:D}", Tpy.Text, wd, core) };
                cmd.Add(tmp);

                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe ipf --in={1}model.oswm --no-ipf_ms1_scoring", Tpy.Text, wd) };
                cmd.Add(tmp);

                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe peptide --context=global --in={1}model.oswm", Tpy.Text, wd) };
                cmd.Add(tmp);

                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe export --in={1}model.oswm", Tpy.Text, wd) };
                cmd.Add(tmp);


                str = "";
                for (int i = 0; i < Lin.Items.Count; ++i)
                {
                    string t = Lin.Items[i].ToString();
                    t = t.Substring(1 + t.LastIndexOf('\\'));
                    str += String.Format("{0}{1}.tsv ", wd, t);
                }

                tmp = new string[] { String.Format("{0}python.exe {0}Scripts\\feature_alignment.py --out {1}aligned.tsv --out_matrix {1}aligned_matrix.tsv --file_format openswath --fdr_cutoff 0.01 --max_fdr_quality 0.2 --mst:useRTCorrection True --mst:Stdev_multiplier 3.0 --method LocalMST --max_rt_diff 30 --alignment_score 0.0001 --frac_selected 0 --realign_method lowess_cython --disable_isotopic_grouping --verbosity 1 --in {2}", Tpy.Text, wd,str) };
                cmd.Add(tmp);
            }
            else
            {

                tmp = new string[Lin.Items.Count];
                str = "";
                for (int i = 0; i < Lin.Items.Count; ++i)
                {
                    string t = Lin.Items[i].ToString();
                    t = t.Substring(1 + t.LastIndexOf('\\'));
                    tmp[i] = String.Format("{0}Scripts\\pyprophet.exe subsample --in={1}{2}.osw --out={1}{2}.osws --subsample_ratio={3:F3}", Tpy.Text, wd, t, 1.0 / Lin.Items.Count);

                    str += String.Format("{0}{1}.osws ", wd, t);
                }
                cmd.Add(tmp);

                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe merge --template={1}lib.os.pqp --out={1}model.oswm {2}", Tpy.Text, wd, str) };
                cmd.Add(tmp);


                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe score --in={1}model.oswm --level=ms1ms2 --threads {2:D}", Tpy.Text, wd, core) };
                cmd.Add(tmp);

                tmp = new string[Lin.Items.Count];
                for (int i = 0; i < Lin.Items.Count; ++i)
                {
                    string t = Lin.Items[i].ToString();
                    t = t.Substring(1 + t.LastIndexOf('\\'));
                    tmp[i] = String.Format("{0}Scripts\\pyprophet.exe score --in={1}{2}.osw --apply_weights={1}model.oswm --level=ms1ms2", Tpy.Text, wd, t);
                }
                cmd.Add(tmp);

                tmp = new string[Lin.Items.Count];
                str = "";
                for (int i = 0; i < Lin.Items.Count; ++i)
                {
                    string t = Lin.Items[i].ToString();
                    t = t.Substring(1 + t.LastIndexOf('\\'));
                    tmp[i] = String.Format("{0}Scripts\\pyprophet.exe reduce --in={1}{2}.osw --out={1}{2}.oswr", Tpy.Text, wd, t);
                    str += String.Format("{0}{1}.oswr ", wd, t);
                }
                cmd.Add(tmp);

                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe merge --template={1}model.oswm --out={1}model_global.oswm {2}", Tpy.Text, wd, str) };
                cmd.Add(tmp);

                tmp = new string[] { String.Format("{0}Scripts\\pyprophet.exe protein --context=global --in={1}model_global.oswm", Tpy.Text, wd) };
                cmd.Add(tmp);

                tmp = new string[Lin.Items.Count];
                for (int i = 0; i < Lin.Items.Count; ++i)
                {
                    string t = Lin.Items[i].ToString();
                    t = t.Substring(1 + t.LastIndexOf('\\'));
                    tmp[i] = String.Format("{0}Scripts\\pyprophet.exe backpropagate --in={1}{2}.osw --apply_scores={1}model_global.oswm", Tpy.Text, wd, t);
                }
                cmd.Add(tmp);

                tmp = new string[Lin.Items.Count];
                str = "";
                for (int i = 0; i < Lin.Items.Count; ++i)
                {
                    string t = Lin.Items[i].ToString();
                    t = t.Substring(1 + t.LastIndexOf('\\'));
                    tmp[i] = String.Format("{0}Scripts\\pyprophet.exe export --no-csv --in={1}{2}.osw  --out={1}{2}.tsv", Tpy.Text, wd, t);
                    str += String.Format("{0}{1}.tsv ", wd, t);
                }
                cmd.Add(tmp);

                tmp = new string[] { String.Format("{0}python.exe {0}Scripts\\feature_alignment.py --out {1}aligned.tsv --out_matrix {1}aligned_matrix.tsv --file_format openswath --method LocalMST --realign_method lowess_cython --max_rt_diff 60 --mst:useRTCorrection True --mst:Stdev_multiplier 3.0 --target_fdr 0.01 --max_fdr_quality 0.05 --verbosity 1 --alignment_score 0.0001 --in {2}", Tpy.Text, wd, str) };
                cmd.Add(tmp);
            }



            tmp = new string[] { String.Format("java TricParser {0}aligned.tsv {1}", wd, Tdb.Text) };
            cmd.Add(tmp);

            tmp = new string[] { String.Format("java UpdateReport {0}aligned.tsv", wd) };
            cmd.Add(tmp);

            button7.Enabled = false;

            cnt = cmd.Count;
            progressBar1.Maximum = cnt;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;

            pro.StartInfo.WorkingDirectory = wd;

            StreamReader sr = new StreamReader(Tirt.Text);
            irtPep = new List<string>();
            while (!sr.EndOfStream)
            {
                irtPep.Add(sr.ReadLine());
            }
            sr.Close();

            runner.RunWorkerAsync();
        }
        int cnt;
        List<string> irtPep = new List<string>();

        private void runner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button7.Enabled = true;
            wd = null;
            progressBar1.Value = progressBar1.Maximum;
        }

        private void runner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ++progressBar1.Value;
        }
        void run(string cmd, string logfn)
        {
            if (cmd.Length == 0)
            {
                return;
            }
            if (cmd[1] != ':')
            {
                pro.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            }
            else
            {
                pro.StartInfo.WorkingDirectory = wd;
            }
            char[] sp = { ' ' };
            string[] c = cmd.Split(sp, 2);
            pro.StartInfo.FileName = c[0];
            pro.StartInfo.Arguments = c[1];
            // String.Format(" 1>>{0}out-{1}.txt 2>>{0}err-{1}.txt", wd, logfn);

            //Console.WriteLine(pro.StartInfo.WorkingDirectory);
            //Console.WriteLine(c[0]);
            //Console.WriteLine(c[1]);
            pro.Start();

            Task<string> o = output(pro.StandardOutput);
            Task<string> e = output(pro.StandardError);
            Directory.CreateDirectory(wd + "log");
            o.Wait();
            write_output(wd + "log\\out-" + logfn + ".txt", o.Result, cmd);

            e.Wait();
            write_output(wd + "log\\err-" + logfn + ".txt", e.Result, cmd);


            pro.WaitForExit();

        }
        void write_output(string fn, string ctx, string cmd)
        {
            if (ctx.Length > 0)
            {
                StreamWriter sw = new StreamWriter(fn);
                sw.WriteLine(cmd);
                sw.WriteLine();
                sw.Write(ctx);
                sw.Close();
            }

        }
        async Task<string> output(StreamReader o)
        {
            string ret = await o.ReadToEndAsync();
            return ret;
        }
        private void runner_DoWork(object sender, DoWorkEventArgs e)
        {

            /*/
            for (int i = 0; i < cmd.Count; ++i)
            {
                string[] cs = cmd.ElementAt(i);

                for (int j = 0; j < cs.Length; ++j)
                {
                    Console.WriteLine(cs[j]);
                }
            }            
            /*/


            //Console.WriteLine(cmd.Count);

            /*/
            if (!Tlib.Text.EndsWith("tsv"))
            {
                for (int i = 0; i < 2; ++i)
                {
                    string[] cs = cmd.ElementAt(i);

                    for (int j = 0; j < cs.Length; ++j)
                    {
                        run(cs[j], String.Format("{0}-{1}",i, j));
                    }
                    ++p;
                    runner.ReportProgress(p);
                }
            }
            else
            {
                File.Copy(Tlib.Text, wd + "lib.os.tsv");
                ++p;
                runner.ReportProgress(p);
                ++p;
                runner.ReportProgress(p);
                //tmp = new string[] { String.Format("copy {0} {1}lib.os.tsv" ,Tlib.Text,wd)};
            }

            for (int i = 0; i < cmd.Count; ++i)
            {
                for (int j = 0; j < cmd.ElementAt(i).Length; ++j)
                {
                    Console.WriteLine(cmd.ElementAt(i)[j]);
                }
            }
            if (Cdry.Checked)
            {
                 return;
            }
            /*/
            int p = 0;


            if (Tlib.Text.EndsWith("tsv") || Tlib.Text.EndsWith("csv"))
            {
                StreamWriter w = new StreamWriter(wd + "lib.os.tsv");
                StreamWriter wirt = new StreamWriter(wd + "irt.tsv");
                StreamReader r = new StreamReader(Tlib.Text);
                bool csv = false;
                bool header = true;
                if (Tlib.Text.EndsWith("csv"))
                {
                    csv = true;
                }
                char[] arr = ", \t\n\r".ToCharArray();
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    string t = line.Trim(arr);
                    if (t.Length == 0)
                    {
                        continue;
                    }
                    if (csv)
                    {
                        t = "";
                        bool flag = false;
                        for (int i = 0; i < line.Length; ++i)
                        {
                            if (line[i] == '"')
                            {
                                flag = !flag;
                                continue;
                            }
                            if (!flag && line[i] == ',')
                            {
                                t += '\t';
                            }
                            else
                            {
                                t += line[i];
                            }
                        }
                    }
                    else
                    {
                        t = line;
                    }
                    if (header)
                    {
                        w.WriteLine(t);
                        wirt.WriteLine(t);
                        header = false;
                        continue;
                    }
                    w.WriteLine(t);
                    for (int i = 0; i < irtPep.Count; ++i)
                    {
                        if (t.Contains("\t" + irtPep.ElementAt(i) + "\t"))
                        {
                            wirt.WriteLine(t);
                            break;
                        }
                    }
                }
                w.Close();
                wirt.Close();
                r.Close();

            }
            else
            {
                for (int i = 0; i < 2; ++i)
                {
                    string[] cs = cmd.ElementAt(i);

                    for (int j = 0; j < cs.Length; ++j)
                    {
                        run(cs[j], String.Format("{0}-{1}", i, j));
                    }
                    ++p;
                    runner.ReportProgress(p);
                }

                StreamWriter w = new StreamWriter(wd + "irt.tsv");
                StreamReader r = new StreamReader(wd + "lib.tsv");
                w.WriteLine(r.ReadLine());
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    for (int i = 0; i < irtPep.Count; ++i)
                    {
                        if (line.Contains("\t" + irtPep.ElementAt(i) + "\t"))
                        {
                            w.WriteLine(line);
                            break;
                        }
                    }
                }
                r.Close();
                w.Close();

            }

            for (int i = p; i < cmd.Count; ++i)
            {
                string[] cs = cmd.ElementAt(i);

                for (int j = 0; j < cs.Length; ++j)
                {
                    run(cs[j], String.Format("{0}-{1}", i, j));
                }
                ++p;
                runner.ReportProgress(p);
            }

        }


    }
}
