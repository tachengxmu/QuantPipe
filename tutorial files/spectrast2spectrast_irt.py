#!C:\Anaconda3\python.exe
"""
Copyright (c) 2012, ETH Zurich, Insitute of Molecular Systems Biology, 
George Rosenberger
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the <organization> nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL ETH ZURICH, INSTITUTE OF MOLECULAR
SYSTEMS BIOLOGY, GEORGE ROSENBERGER BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
"""
from __future__ import print_function
import sys
import os
import csv
import getopt
import scipy
from numpy import *
from scipy import stats
import matplotlib as mpl
mpl.use('Agg')
import matplotlib.pyplot as plt
#import profile 

from msproteomicstoolslib.math.chauvenet import *

def lmedian(valarr):
  vals = sorted(valarr)
  if len(vals) % 2 == 1:
    return vals[(len(vals) + 1) // 2 - 1]
  else:
    return vals[len(vals) // 2 - 1]

def all_indices(value, qlist):
  indices = []
  idx = -1
  while True:
    try:
      idx = qlist.index(value, idx+1)
      indices.append(idx)
    except ValueError:
      break
  return indices

class sptxtio(object):
  def __init__(self):
    self.header = []
    self.spectra = []
    self.blocks = []
    self.spectrablocks = []
    self.rtkit = {}
    self.rt_all = {}
    self.prob_all = {}
    self.rt = {}
    self.rt_run = {}
    self.irt = {}
    self.irt_merged = {}
    self.a = {}
    self.b = {}
    self.rsq = {}
    self.spectrum_block_map = {}
  
  def input(self,file,precursorlevel,spectralevel):
    try:
      sptxt_infile = open(file, 'r')
    except IOError:
      print(file, "not readable")
    
    sptxt_header = []
    sptxt_block = []
    for sptxt_line in sptxt_infile:
      if sptxt_line[0] == "#":
        sptxt_header.append(sptxt_line)
      else:
        sptxt_block.append(sptxt_line)
        if sptxt_line == "\n":
          if precursorlevel:
            unique_identifier = sptxt_block[0].split("Name: ")[1].split("\n")[0]
          elif spectralevel:
            unique_identifier = sptxt_block[6].split("RawSpectrum=")[1].split(" ")[0]
          else:
            unique_identifier = sptxt_block[0].split("Name: ")[1].split("/")[0]
          sequence = sptxt_block[0].split("Name: ")[1].split("/")[0]

          block = blockio(sptxt_block[1].split("LibID: ")[1].split("\n")[0],
                            unique_identifier, sequence,
                            sptxt_block[0].split("Name: ")[1].split("/")[1].split("\n")[0],
                            sptxt_block[6].split("Mods=")[1].split(" ")[0],
                            sptxt_block[6].split("RawSpectrum=")[1].split(".")[0],
                            float(sptxt_block[6].split("RetentionTime=")[1].split(",")[0]),
                            float(sptxt_block[6].split("Prob=")[1].split(" ")[0]),
                          sptxt_block)
          self.push(block)
          sptxt_block = []
    
    sptxt_infile.close()
    self.pushheader(sptxt_header)
  
  def output(self,file):
    try:
      sptxt_outfile = open(file, 'w')
      sptxt_outfile.writelines(self.header)
      newblocks = []
      for block in self.blocks:
        block.binindex = sptxt_outfile.tell()
        sptxt_outfile.writelines(block.block)
        newblocks.append(block)
      self.blocks = newblocks
    except IOError:
      print(file, "not readable")
    sptxt_outfile.close()

  def report(self,file):
    try:
      report_outfile = open(file, 'w')
      report = csv.writer(report_outfile, delimiter=',')
      report.writerow(['peptide','rt','irt','rt_lmedian','rt_mean','rt_sd','irt_lmedian','irt_mean','irt_sd','rt_run_lmedian','rt_run_mean','rt_run_sd'])
      ind = {}
      for spectrum in self.irt:
        for peptide in self.irt[spectrum]:
          if peptide not in ind:
            ind[peptide] = []
          ind[peptide].append(spectrum)

      for peptide in ind:
        rt = []
        irt = []
        rt_run_median = []
        rt_run_mean = []
        rt_run_sd = []
        for spectrum in ind[peptide]:
          rt.append(self.rt[spectrum][peptide])
          irt.append(self.irt[spectrum][peptide])
          rt_run_median.append(spectrum+":"+str(lmedian(self.rt_run[spectrum][peptide])))
          rt_run_mean.append(spectrum+":"+str(mean(self.rt_run[spectrum][peptide])))
          rt_run_sd.append(spectrum+":"+str(std(self.rt_run[spectrum][peptide])))
        report.writerow([peptide,lmedian(rt),self.irt_merged[peptide],lmedian(rt),mean(rt),std(rt),lmedian(irt),mean(irt),std(irt),";".join(rt_run_median),";".join(rt_run_mean),";".join(rt_run_sd)])
    except IOError:
      print(file, "not readable")
    report_outfile.close()

  def push(self,block):
    self.spectra.append(block.rawspectrum)
    self.spectra = list(set(self.spectra))
    self.blocks.append(block)

    if block.rawspectrum not in self.spectrablocks:
      self.spectrablocks.append(block.rawspectrum)
      self.rtkit[block.rawspectrum] = []
      self.rt_all[block.rawspectrum] = {}
      self.prob_all[block.rawspectrum] = {}
      self.rt[block.rawspectrum] = {}
      self.rt_run[block.rawspectrum] = {}
      self.irt[block.rawspectrum] = {}
      self.a[block.rawspectrum] = float()
      self.b[block.rawspectrum] = float()
      self.rsq[block.rawspectrum] = float()
      self.spectrum_block_map[block.rawspectrum] = {}

    self.spectrum_block_map[block.rawspectrum][block.peptide] = block
    
    if block.peptide not in self.rt_all[block.rawspectrum]:
      self.rt_all[block.rawspectrum][block.peptide] = []
      self.prob_all[block.rawspectrum][block.peptide] = []
    self.rt_all[block.rawspectrum][block.peptide].append(block.rt)
    self.prob_all[block.rawspectrum][block.peptide].append(block.prob)
  
  def pushheader(self,header):
    self.header = header
  
  def merge(self,rmout):
    for rawspectrum in self.rt_all:
      for peptide in self.rt_all[rawspectrum]:
        rt = []
        for idx in all_indices(sorted(self.prob_all[rawspectrum][peptide], reverse=True)[0],self.prob_all[rawspectrum][peptide]):
          rt.append(self.rt_all[rawspectrum][peptide][idx])

        if rmout and len(rt) > 2:
          self.rt[rawspectrum][peptide] = lmedian(array(rt)[chauvenet(array(rt),array(rt))])
          self.rt_run[rawspectrum][peptide] = array(rt)[chauvenet(array(rt),array(rt))]
        else:
          self.rt[rawspectrum][peptide] = lmedian(rt)
          self.rt_run[rawspectrum][peptide] = rt
  
  def calibrate(self,rtkit,outliers,surrogates,linregs,rsq_threshold):
    missingirt = []
    for rawspectrum in self.spectra:
      rt_calibration = []
      irt_calibration = []
      print("RT peptides used per run:")
      print("run\tpeptide\trt\tirt")
      for peptide in self.rt[rawspectrum]:
        peptide_sequence = self.spectrum_block_map[rawspectrum][peptide].sequence
        if peptide_sequence in rtkit:
          if rawspectrum in outliers:
            if peptide_sequence not in outliers[rawspectrum]:
              rt_calibration.append(self.rt[rawspectrum][peptide])
              irt_calibration.append(rtkit[peptide_sequence])
              print(rawspectrum + "\t" + peptide_sequence + "\t" + str(self.rt[rawspectrum][peptide]) + "\t" + str(rtkit[peptide_sequence]))
          else:
            rt_calibration.append(self.rt[rawspectrum][peptide])
            irt_calibration.append(rtkit[peptide_sequence])
            print(rawspectrum + "\t" + peptide_sequence + "\t" + str(self.rt[rawspectrum][peptide]) + "\t" + str(rtkit[peptide_sequence]))

      if len(rt_calibration) < 2:
        missingirt.append(rawspectrum)

      if len(rt_calibration) >= 2:
        if rawspectrum in linregs:
          print("Replacing iRT normalization of run " + rawspectrum + " with c: " + str(linregs[rawspectrum]['b']) + " m: " + str(linregs[rawspectrum]['a']) + ".")
          self.a[rawspectrum] = linregs[rawspectrum]['a']
          self.b[rawspectrum] = linregs[rawspectrum]['b']
          self.rsq[rawspectrum] = 1.0
        else:
          slope, intercept, r_value, p_value, std_err = stats.linregress(rt_calibration,irt_calibration)
          (self.a[rawspectrum], self.b[rawspectrum]) = (slope, intercept)
          self.rsq[rawspectrum] = r_value**2
          plt.figure()
          plt.title(rawspectrum + " (c: " + str(self.b[rawspectrum]) + ", m: " + str(self.a[rawspectrum]) + ")")
          fit_fn = scipy.poly1d((self.a[rawspectrum],self.b[rawspectrum]))
          plt.plot(rt_calibration,irt_calibration, 'b.',rt_calibration,fit_fn(rt_calibration),'-r')
          plt.savefig(rawspectrum+'.png')

    for host in surrogates:
      print("Replacing iRT normalization of run " + host + " with " + surrogates[host] + ".")
      self.a[host] = self.a[surrogates[host]]
      self.b[host] = self.b[surrogates[host]]
      self.rsq[host] = self.rsq[surrogates[host]]
      missingirt = [a for a in missingirt if a != host]

    if len(missingirt) > 0:
      print("Did you search for the true sequences?")
      print("Did you use a non-consensus and without best replicates summarization spectral library?")
      print("Did you add the Biognosys RT-kit to all of your samples?")
      print("The following runs don't contain peptides from the Biognosys RT-kit:",missingirt)
      raise Exception("Error: At least one of your runs doesn't contain any peptides from the Biognosys RT-kit!")

    for rawspectrum in self.spectra:
      if self.rsq[rawspectrum] < rsq_threshold:
        raise Exception("Error: R-squared " + str(self.rsq[rawspectrum]) + " of run " + rawspectrum + " is below the threshold of " + str(rsq_threshold) + ".")

  def transform(self,rmout):
    irt = {}
    for rawspectrum in self.rt:
      for peptide in self.rt[rawspectrum]:
        self.irt[rawspectrum][peptide] = scipy.polyval([self.a[rawspectrum],self.b[rawspectrum]],self.rt[rawspectrum][peptide])
        if peptide not in irt:
          irt[peptide] = []
        irt[peptide].append(self.irt[rawspectrum][peptide])

    for peptide in irt:
      if len(irt) == 1:
        self.irt_merged[peptide] = round(irt[peptide][0],5)
      else:
        if rmout and len(irt) > 2:
          self.irt_merged[peptide] = round(lmedian(array(irt[peptide])[invert(chauvenet(array(irt[peptide]),array(irt[peptide])))]),5)
        else:
          self.irt_merged[peptide] = round(lmedian(irt[peptide]),5)
 
    for i in range(0,len(self.blocks)):
      self.blocks[i].replace(self.irt_merged[self.blocks[i].peptide])

class blockio(object):
  def __init__(self,libid,peptide,sequence,charge,mods,rawspectrum,rt,prob,block):
    self.libid = libid
    self.peptide = peptide
    self.sequence = sequence
    self.charge = charge
    self.mods = mods
    self.binindex = -1
    self.rawspectrum = rawspectrum
    self.rt = rt
    self.prob = prob
    self.irt = float()
    self.block = block
  
  def replace(self,irt):
    self.irt = irt
    block_new = []
    for line in self.block[:6]:
      block_new.append(line)
    
    block_new.append(self.block[6][:self.block[6].find("RetentionTime=")+len("RetentionTime=")]+str(self.irt)+","+str(self.irt)+","+str(self.irt)+self.block[6][self.block[6][self.block[6].find("RetentionTime="):].find(" ")+self.block[6].find("RetentionTime="):])
  
    for line in self.block[7:]:
      block_new.append(line)
    self.block = block_new

class pepidxio(object):
  def __init__(self,blocks):
    self.blocks = blocks
    self.pepindex = {}
    
  def pepind(self):
    for block in self.blocks:
      id = block.sequence + "\t" + block.charge + "|" + block.mods + "|\t"
      if id not in self.pepindex:
        self.pepindex[id] = []
      self.pepindex[id].append(block.binindex)
    
  def output(self,pepidx_outfile):
    try:
      pepidx_out = open(pepidx_outfile, 'w')
      for key in self.pepindex:
        line = key + ' '.join(str(x) for x in self.pepindex[key]) + "\n"
        pepidx_out.write(line)
    except IOError:
      print(pepidx_outfile, "not readable")
    pepidx_out.close()

# MAIN
def main(argv):
  rtkit = {'ADTLDPALLRPGR':35.987015,'AFEEAEK':-21.35736,'AFLIEEQK':22.8,'AGFAGDDAPR':-9.819255,'AGLQFPVGR':37.04898,'AILGSVER':5.415375,'APGFGDNR':-15.63474,'AQIWDTAGQER':16.854875,'ATAGDTHLGGEDFDNR':3.185526667,'ATIGADFLTK':43.837285,'AVANQTSATFLR':19.24765,'AVFPSIVGRPR':34.03497,'C[160]ATITPDEAR':-10.13943,'DAGTIAGLNVLR':59.03744667,'DAHQSLLATR':-3.25497,'DELTLEGIK':39.132035,'DLMAC[160]AQTGSGK':0.306955,'DLTDYLMK':60.01111111,'DNIQGITKPAIR':12.598125,'DNTGYDLK':-9.39721,'DSTLIMQLLR':103.65,'DSYVGDEAQSK':-15.509125,'DVQEIFR':29.61571,'DWNVDLIPK':70.53546,'EAYPGDVFYLHSR':46.35,'EC[160]ADLWPR':28.711905,'EDAANNYAR':-23.23042,'EGIPPDQQR':-15.8411,'EHAALEPR':-22.61094,'EIAQDFK':-4.04913,'EIQTAVR':-17.07064,'ELIIGDR':11.56179,'ELISNASDALDK':23.50069,'EMVELPLR':47.96546,'ESTLHLVLR':28.54494,'EVDIGIPDATGR':37.10299,'FDDGAGGDNEVQR':-11.31703,'FDLMYAK':38.2,'FDNLYGC[160]R':9.6064,'FEELC[160]ADLFR':73.5,'FELSGIPPAPR':52.5,'FELTGIPPAPR':53.1,'FPFAANSR':18.76225,'FQSLGVAFYR':60.2276,'FTQAGSEVSALLGR':61.450335,'FTVDLPK':37.86026,'FVIGGPQGDAGLTGR':40.551975,'GC[160]EVVVSGK':-15.49014,'GEEILSGAQR':-1.811165,'GILFVGSGVSGGEEGAR':51.15,'GILLYGPPGTGK':45.36582,'GIRPAINVGLSVSR':37.98295,'GNHEC[160]ASINR':-23.57003,'GVC[160]TEAGMYALR':31.20584,'GVLLYGPPGTGK':28.11667,'GVLMYGPPGTGK':28.20674,'HFSVEGQLEFR':41.108635,'HITIFSPEGR':22.39813,'HLQLAIR':9.42694,'HLTGEFEK':-13.72484,'HVFGQAAK':-24.54245,'IC[160]DFGLAR':28.009545,'IC[160]GDIHGQYYDLLR':50.34788,'IETLDPALIRPGR':43.43414,'IGGIGTVPVGR':21.9,'IGLFGGAGVGK':43.285185,'IGPLGLSPK':29.48313,'IHETNLK':-25.53888,'IINEPTAAAIAYGLDK':65.72006667,'IYGFYDEC[160]K':31.695135,'KPLLESGTLGTK':9.057185,'LAEQAER':-25.089125,'LGANSLLDLVVFGR':134.00759,'LIEDFLAR':56.93148,'LILIESR':28.145215,'LPLQDVYK':29.2,'LQIWDTAGQER':36.28872,'LVIVGDGAC[160]GK':10.8,'LVLVGDGGTGK':12.022895,'LYQVEYAFK':46.26582,'MLSC[160]AGADR':-15.49156,'NILGGTVFR':49.61455,'NIVEAAAVR':5.73971,'NLLSVAYK':34.34229,'NLQYYDISAK':25.8,'NMSVIAHVDHGK':-5.36295,'QAVDVSPLR':11.34271,'QTVAVGVIK':9.9,'SAPSTGGVK':-27.56682,'SGQGAFGNMC[160]R':0.790505,'SNYNFEKPFLWLAR':96.01717,'STELLIR':18.1,'STTTGHLIYK':-9.48751,'SYELPDGQVITIGNER':67.30002,'TIAMDGTEGLVR':32.822885,'TIVMGASFR':29.53023,'TLSDYNIQK':4.35,'TTIFSPEGR':15.183785,'TTPSYVAFTDTER':33.79824667,'TTVEYLIK':30.16799,'VAVVAGYGDVGK':15.331395,'VC[160]ENIPIVLC[160]GNK':49.065875,'VLPSIVNEVLK':83.750085,'VPAINVNDSVTK':17.70942,'VSTEVDAR':-20.136945,'VVPGYGHAVLR':8.61752,'WPFWLSPR':98.382385,'YAWVLDK':41.6,'YDSTHGR':-57.05955,'YFPTQALNFAFK':95.4,'YLVLDEADR':27.6947,'YPIEHGIVTNWDDMEK':56.9,'YTQSNSVC[160]YAK':-12.794935,'AGGSSEPVTGLADK':0,'VEATFGVDESANK':10.03,'YILAGVESNK':16.82,'TPVISGGPYYER':23.54,'TPVITGAPYYER':28.18,'GDLDAASYYAPVR':38.67,'DAVTPADFSEWSK':50.27,'TGFIIDPGGVIR':68.66,'GTFIIDPAAIVR':85.38,'FLLQFGAQGSPLFK':100,'ALELDSNLYR':38.3,'ALVAYYQK':10,'VSHLLGINVTDFTR':67.1,'ENFYQLQLR':48.7,'LLGVEGTTLR':33.4,'LALDIEIATYR':88.9,'LAVNMVPFPR':60.9,'YFPTQALNFAFK':94.1,'AGVLAHLEEER':23,'LDPHLVLDQLR':60.9,'AATIVIQSYLR':55.5,'AAFGISDSYVDGSS[167]FDPQRR':57.45511,'AAFGISDSYVDGS[167]SFDPQRR':57.4,'AAFGIS[167]DSYVDGSSFDPQRR':64,'C[160]RS[167]PGMLEPLGSAR':32.2,'DKFS[167]PTQDRPESSTVLK':9.9,'DKFS[167]PTQDRPESSTVLKVTPR':17.6,'DLQSS[167]ERVSWR':25.99112,'DTPQTPS[167]RGRSEC[160]DSSPEPK':-28.92846,'GS[167]LSRSSSPVTELTAR':32.5,'IPAASAAAMNLAS[167]AR':38.6,'IPAAS[167]AAAMNLASAR':46.6,'LGLIQEDVASSC[160]IPR':56.9628,'MELGT[181]PLR':35.5,'MSC[160]FSRPSMS[167]PTPLDR':36.4,'MSQVPAPVPLMSLR':77.0474,'MSQVPAPVPLMS[167]LR':84.4,'MS[167]QVPAPVPLMSLR':90.2,'MS[167]QVPAPVPLM[147]SLR':65.2,'MTSERERAPS[167]PASR':-32.73406,'MVQAS[167]SQSLLPPAQDRPR':26.9,'S[167]LSYSPVER':21.3,'RVPS[167]PTPVPK':-12.52342,'SSS[167]PVTELTAR':24.9,'AQSGTDSS[167]PEHKIPAPR':-15.59084,'TPAAS[167]AVNLAGAR':22,'S[167]PGMLEPLGSAR':50.6,'S[167]SSELSPEVVEK':23.5,'VSS[167]PVLETVQQR':21.9,'TS[167]AIPASVNLADSR':37,}

  splib_in = False
  splib_out = False
  outliers={}
  surrogates={}
  linregs={}
  rmout = False
  precursorlevel = False
  spectralevel = False
  report = False
  rsq_threshold = 0.1

  help = False
  try:
    opts, args = getopt.getopt(argv, "i:o:k:apre:s:l:t:",["in=","out=","kit=","applychauvenet","spectralevel", "precursorlevel","report","exclude=","surrogate=","linearregressions=","rsq_threshold="])
  except getopt.GetoptError:
    help = True
    opts =  ( ("",""),)

  for opt, arg in opts:
    if opt in ("-i","--in"):
      splib_in = arg
    elif opt in ("-o","--out"):
      splib_out = arg
      pepidx_out = os.path.splitext(arg)[0] + '.pepidx'
      report_out = os.path.splitext(arg)[0] + '.csv'
    elif opt in ("-a","--applychauvenet"):
      rmout = True
    elif opt in ("-k","--kit"):
      #rtkit = {}
      for peptide in arg.split(","):
        rtkit[peptide.split(":")[0]] = float(peptide.split(":")[1])
    elif opt in ("-p","--precursorlevel"):
      precursorlevel = True
    elif opt in ("--spectralevel"):
      spectralevel = True
    elif opt in ("-r","--report"):
      report = True
    elif opt in ("-e","--exclude"):
      for outlier in arg.split(","):
        if outlier.split(":")[0] not in outliers:
          outliers[outlier.split(":")[0]] = []
        outliers[outlier.split(":")[0]].append(outlier.split(":")[1])
    elif opt in ("-s","--surrogate"):
      for surrogate in arg.split(","):
        surrogates[surrogate.split(":")[0]] = surrogate.split(":")[1]
    elif opt in ("-l","--linearregression"):
      for linreg in arg.split(","):
        linregs[linreg.split(":")[0]] = {}
        linregs[linreg.split(":")[0]]['a'] = float(linreg.split(":")[1].split("/")[1])
        linregs[linreg.split(":")[0]]['b'] = float(linreg.split(":")[1].split("/")[0])
    elif opt in ("-t","--rsq_threshold"):
        rsq_threshold = float(arg)

  if help or not splib_in or not splib_out:
    print("SpectraST RT Normalizer")
    print("---------------------------------------------------------------------------------------------")
    print("Usage:     spectrast2spectrast_irt.py -i non_consensus_library.[splib/sptxt] -o non_consensus_library_irt.splib")
    print("Input:     SpectraST non_consensus_library.splib in txt format")
    print("Output:    SpectraST non_consensus_library_irt.[splib/pepidx] and regression plots for all runs.")
    print("Argument:  -i [--in]: input file")
    print("           -o [--out]: output file")
    print("           (optional) -k [--kit]: specifiy RT-kit [LGGNEQVTR:-28.3083,GAGSSEPVTGLDAK:0.227424,VEATFGVDESNAK:13.1078,YILAGVENSK:22.3798,TPVISGGPYEYR:28.9999,TPVITGAPYEYR:33.6311,DGLDAASYYAPVR:43.2819,ADVTPADFSEWSK:54.969,GTFIIDPGGVIR:71.3819,GTFIIDPAAVIR:86.7152,LFLQFGAQGSPFLK:98.0897]")
    print("           (optional) -a [--applychauvenet]: should Chavenet's criterion be used to exclude outliers?")
    print("           (optional) -p [--precursorlevel]: should precursors instead of peptides be used for grouping?")
    print("           (optional)    [--spectralevel]: do not merge or group any peptides or precursors (use raw spectra)")
    print("           (optional) -r [--report]: should a csv report be written?")
    print("           (optional) -e [--exclude]: specify peptides from the RT-kit to exclude [run_id1:LGGNEQVTR,run_id2:GAGSSEPVTGLDAK]")
    print("           (optional) -s [--surrogate]: specify surrogate calibrations [broken_run_id1:working_run_id2]")
    print("           (optional) -l [--linearregression]: specify surrogate linear regressions (first number: c, second number: m) [broken_run_id1:1/3]")
    print("           (optional) -t [--rsq_threshold]: specify r-squared threshold to accept linear regression [0.95]")
    print("Important: The splib need to be in txt format!")
    print("           spectrast -c_BIN! -cNnon_consensus.txt non_consensus.bin.splib")
    print("           All runs in your library further need to contain the Biognosys RT-kit peptides.")
    print("Contact:   George Rosenberger <rosenberger@imsb.biol.ethz.ch>")
    sys.exit()

  # splib containing all spectra
  sptxt_all = sptxtio() # create object
  sptxt_all.input(splib_in, precursorlevel, spectralevel) # read sptxt
  sptxt_all.merge(rmout) # merge retention times of same peptides in each run individually
  sptxt_all.calibrate(rtkit,outliers,surrogates,linregs,rsq_threshold) # calibrate using the retention kit
  sptxt_all.transform(rmout) # transform retention times to iRT
  sptxt_all.output(splib_out) # write sptxt

  # write pepidx
  pepidx = pepidxio(sptxt_all.blocks)
  pepidx.pepind()
  pepidx.output(pepidx_out)

  # write report
  if report:
    sptxt_all.report(report_out)

#profile.run('main()')
if __name__ == "__main__":
  main(sys.argv[1:])
