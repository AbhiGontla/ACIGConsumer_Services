using Core;
using Core.Api;
using Core.Domain;
using Core.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class ACIGDbContext : DbContext
    {

        public ACIGDbContext()
        {

        }
        public ACIGDbContext(DbContextOptions<ACIGDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().
                    UseSqlServer(@"Data Source=DESKTOP-D4V62HB;Initial Catalog=ACIG;Trusted_Connection=True;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(builder =>
            {
                builder.Property(e => e.FirstName).HasMaxLength(50);

                builder.Property(e => e.MiddleName).HasMaxLength(50);

                builder.Property(e => e.LastName).HasMaxLength(50);

                builder.Property(e => e.EmailId).HasMaxLength(50);

                builder.Property(e => e.MobileNumber).HasMaxLength(50);


            });
            //Registration Table
            modelBuilder.Entity<Registration>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.DOB);
                builder.Property(e => e.MemberName);
                builder.Property(e => e.MemberMobileNumber);
                builder.Property(e => e.MemberStatus);
                builder.Property(e => e.MemberType);
                builder.Property(e => e.TPAID);
                builder.Property(e => e.TPAName);
                builder.Property(e => e.PolicyNo);
                builder.Property(e => e.PolicyFromDate);
                builder.Property(e => e.PolicyToDate);
                builder.Property(e => e.TushfaMemberNo);
                builder.Property(e => e.CardNo);
                builder.Property(e => e.PhoneNoVerification);
                builder.Property(e => e.CreatePin);
                builder.Property(e => e.ConfirmPin);
                builder.Property(e => e.LoginDateandTime);
                builder.Property(e => e.Iqama_NationalID);
                builder.Property(e => e.ClassCode);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.NetworkCode);
                builder.Property(e => e.NetworkName);
            });
            //Registration Table
            modelBuilder.Entity<Approvals>(builder =>
            {
                builder.Property(e => e.CL_ADMDATE);
                builder.Property(e => e.CL_CLMAMT);
                builder.Property(e => e.CL_DATEIN);
                builder.Property(e => e.CL_DATEOT);
                builder.Property(e => e.CL_DECISION);
                builder.Property(e => e.CL_DIAG);
                builder.Property(e => e.CL_DISCHARGE);
                builder.Property(e => e.CL_EXP_DISCH);
                builder.Property(e => e.CL_HOSFILE);
                builder.Property(e => e.CL_NAME);
                builder.Property(e => e.CL_PP_NO);
                builder.Property(e => e.CL_PROVIDER);
                builder.Property(e => e.CL_PROV_NAME);
                builder.Property(e => e.CL_REJ);
                builder.Property(e => e.CL_REMK);
                builder.Property(e => e.CL_RISK);
                builder.Property(e => e.CL_SEQID);
                builder.Property(e => e.CL_VATAMT);
                builder.Property(e => e.Code);
                builder.Property(e => e.Id);
                builder.Property(e => e.InsPolicyNo);
                builder.Property(e => e.NationalId);
                builder.Property(e => e.POLICY_SEQ);
                builder.Property(e => e.YearofBirth);
            });
            //Policies Table
            modelBuilder.Entity<Policies>(builder =>
            {
                builder.Property(e => e.AdditionPremium);
                builder.Property(e => e.BrokerName);
                builder.Property(e => e.CardNo);
                builder.Property(e => e.CCHIErrorMessage);
                builder.Property(e => e.CCHIStatus);
                builder.Property(e => e.CCHIUploadDate);
                builder.Property(e => e.CityCode);
                builder.Property(e => e.CityDesc);
                builder.Property(e => e.ClassCode);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.ClientName);
                builder.Property(e => e.ClientType);
                builder.Property(e => e.DeletionDate);
                builder.Property(e => e.DeletionPremium);
                builder.Property(e => e.DeletionReason);
                builder.Property(e => e.DOBGreg);
                builder.Property(e => e.DOBHijri);
                builder.Property(e => e.EmployeeNo);
                builder.Property(e => e.EnrollmentDate);
                builder.Property(e => e.Gender);
                builder.Property(e => e.Id);
                builder.Property(e => e.IDExpiryDate);
                builder.Property(e => e.Iqama_NationalID);
                builder.Property(e => e.MaritalStatus);
                builder.Property(e => e.MemberName);
                builder.Property(e => e.MemberStatus);
                builder.Property(e => e.MemberTypeCode);
                builder.Property(e => e.MemberTypeDesc);
                builder.Property(e => e.MigrationDate);
                builder.Property(e => e.MigrationPremium);
                builder.Property(e => e.MobileNumber);
                builder.Property(e => e.NationalityCode);
                builder.Property(e => e.NationalityDesc);
                builder.Property(e => e.NetworkCode);
                builder.Property(e => e.NetworkName);
                builder.Property(e => e.OccupationCode);
                builder.Property(e => e.OccupationDesc);
                builder.Property(e => e.PolicyFromDate);
                builder.Property(e => e.PolicyNumber);
                builder.Property(e => e.PolicySponsorName);
                builder.Property(e => e.PolicyToDate);
                builder.Property(e => e.PolicyType);
                builder.Property(e => e.RegionCode);
                builder.Property(e => e.RegionDesc);
                builder.Property(e => e.RelationCode);
                builder.Property(e => e.RelationDesc);
                builder.Property(e => e.SponsorID);
                builder.Property(e => e.TPAID);
                builder.Property(e => e.TPAName);
                builder.Property(e => e.TransDate);
                builder.Property(e => e.TushfaMemberNo);
            });

            //CoverageBalance Table
            modelBuilder.Entity<CoverageBalance>(builder =>
            {
                builder.Property(e => e.Code);
                builder.Property(e => e.Id);
                builder.Property(e => e.InsPolicyNo);
                builder.Property(e => e.NationalID);
                builder.Property(e => e.Description);
                builder.Property(e => e.Limit);
                builder.Property(e => e.RemainingAmount);
                builder.Property(e => e.YearofBirth);
            });
            //Providers Table
            modelBuilder.Entity<Providers>(builder =>
            {
                builder.Property(e => e.Code);
                builder.Property(e => e.Id);
                builder.Property(e => e.InsPolicyNo);
                builder.Property(e => e.NationalID);
                builder.Property(e => e.YearofBirth);
                builder.Property(e => e.ProviderName);
                builder.Property(e => e.ProviderNumber);
                builder.Property(e => e.ProviderStatus);
                builder.Property(e => e.ProviderType);
                builder.Property(e => e.CCHINumber);
                builder.Property(e => e.CCHIExpiryDate);
                builder.Property(e => e.Status);

            });
            //PaidClaims
            modelBuilder.Entity<PaidClaims>(builder =>
            {
                builder.Property(e => e.ADM_TYPE);
                builder.Property(e => e.CL_ACCIDATE);
                builder.Property(e => e.CL_ADMDATE);
                builder.Property(e => e.CL_BATCH);
                builder.Property(e => e.CL_BATCH_STS);
                builder.Property(e => e.CL_CALCAMT_LL);
                builder.Property(e => e.CL_CALCAMT_OR);
                builder.Property(e => e.CL_CCHINO);
                builder.Property(e => e.CL_ClASS);
                builder.Property(e => e.CL_CLMAMT_LL);
                builder.Property(e => e.CL_CLMAMT_OR);
                builder.Property(e => e.CL_CLMTYPE);
                builder.Property(e => e.CL_COMPANY);
                builder.Property(e => e.CL_COUNTRY);
                builder.Property(e => e.CL_CURR);
                builder.Property(e => e.CL_DCLDTE);
                builder.Property(e => e.CL_DEDCTBL_LL);
                builder.Property(e => e.CL_DEDCTBL_OR);
                builder.Property(e => e.CL_DEDCTN_LL);
                builder.Property(e => e.CL_DEDCTN_OR);
                builder.Property(e => e.CL_DEDMED);
                builder.Property(e => e.CL_DEDPROV);
                builder.Property(e => e.CL_DEDREASON);
                builder.Property(e => e.CL_DIAG);
                builder.Property(e => e.CL_DIAG_DESC);
                builder.Property(e => e.CL_DISCHARGE);
                builder.Property(e => e.CL_FILENO);
                builder.Property(e => e.CL_FSTVSA);
                builder.Property(e => e.CL_FTYPE);
                builder.Property(e => e.CL_HOSPAMT_LL);
                builder.Property(e => e.CL_HOSPAMT_OR);
                builder.Property(e => e.CL_INSINSURD);
                builder.Property(e => e.CL_INSPOLNO);
                builder.Property(e => e.CL_INV_DATE);
                builder.Property(e => e.CL_INV_NO);
                builder.Property(e => e.CL_INV_RDATE);
                builder.Property(e => e.CL_PAIDAMT_LL);
                builder.Property(e => e.CL_PAIDAMT_OR);
                builder.Property(e => e.CL_PAYABLE_LL);
                builder.Property(e => e.CL_PAYABLE_OR);
                builder.Property(e => e.CL_PP_NO);
                builder.Property(e => e.CL_PROD);
                builder.Property(e => e.CL_PROVIDERNO);
                builder.Property(e => e.CL_PROVIDERTYPE);
                builder.Property(e => e.CL_PROVNAME);
                builder.Property(e => e.CL_RISK);
                builder.Property(e => e.CL_SEQID);
                builder.Property(e => e.CL_SRVC);
                builder.Property(e => e.CL_STATUS);
                builder.Property(e => e.CL_STLDATE);
                builder.Property(e => e.CL_STSER);
                builder.Property(e => e.CL_SUBADM);
                builder.Property(e => e.CL_SUBOFF);
                builder.Property(e => e.CL_SUBRISK);
                builder.Property(e => e.CL_SYSDATE);
                builder.Property(e => e.CL_TRFTUI);
                builder.Property(e => e.CL_VATAMT);
                builder.Property(e => e.CL_VATNET);
                builder.Property(e => e.CL_VISA);
                builder.Property(e => e.CL_VISA_CODE);
                builder.Property(e => e.CL_VISA_STS);
                builder.Property(e => e.Code);
                builder.Property(e => e.CONGINATAL_CHK);
                builder.Property(e => e.EMERGENCY_CHK);
                builder.Property(e => e.FROMDATE);
                builder.Property(e => e.GROSS_LL);
                builder.Property(e => e.GROSS_OR);
                builder.Property(e => e.Id);
                builder.Property(e => e.InsPolicyNo);
                builder.Property(e => e.LOGINDATE);
                builder.Property(e => e.NationalID);
                builder.Property(e => e.NETPAYABLE_LL);
                builder.Property(e => e.NETPAYABLE_OR);
                builder.Property(e => e.POLICY_INC);
                builder.Property(e => e.POLICY_SEQ);
                builder.Property(e => e.PRE_DISEASE_CHK);
                builder.Property(e => e.PROVIDER_CITY);
                builder.Property(e => e.SERIAL);
                builder.Property(e => e.SRV_DESC);
                builder.Property(e => e.STATUS);
                builder.Property(e => e.TODATE);
                builder.Property(e => e.YearofBirth);
            });
            //OSClaims
            modelBuilder.Entity<OSClaims>(builder =>
            {
                builder.Property(e => e.ADM_TYPE);
                builder.Property(e => e.CL_ACCIDT);
                builder.Property(e => e.CL_ADMDATE);
                builder.Property(e => e.CL_BATCH);
                builder.Property(e => e.CL_BATCH_STS);
                builder.Property(e => e.CL_CALCAMT_LL);
                builder.Property(e => e.CL_CALCAMT_OR);
                builder.Property(e => e.CL_CCHINO);
                builder.Property(e => e.CL_CLASS);
                builder.Property(e => e.CL_CLMAMT_LL);
                builder.Property(e => e.CL_CLMAMT_OR);
                builder.Property(e => e.CL_CLMTYPE);
                builder.Property(e => e.CL_COUNTRY);
                builder.Property(e => e.CL_CURR);
                builder.Property(e => e.CL_DCLDTE);
                builder.Property(e => e.CL_DEDCTBL_LL);
                builder.Property(e => e.CL_DEDCTBL_OR);
                builder.Property(e => e.CL_DEDCTN_LL);
                builder.Property(e => e.CL_DEDCTN_OR);
                builder.Property(e => e.CL_DEDMED);
                builder.Property(e => e.CL_DEDPROV);
                builder.Property(e => e.CL_DEDREASON);
                builder.Property(e => e.CL_DIAG);
                builder.Property(e => e.CL_DIAG_DESC);
                builder.Property(e => e.CL_DISCHARGE);
                builder.Property(e => e.CL_FILENO);
                builder.Property(e => e.CL_FSTVSA);
                builder.Property(e => e.CL_FTYPE);
                builder.Property(e => e.CL_HOSPAMT_LL);
                builder.Property(e => e.CL_HOSPAMT_OR);
                builder.Property(e => e.CL_INSINSURD);
                builder.Property(e => e.CL_INSPOLNO);
                builder.Property(e => e.CL_INV_DATE);
                builder.Property(e => e.CL_INV_NO);
                builder.Property(e => e.CL_INV_RDATE);
                builder.Property(e => e.CL_PAIDAMT_LL);
                builder.Property(e => e.CL_PAIDAMT_OR);
                builder.Property(e => e.CL_PAYABLE_LL);
                builder.Property(e => e.CL_PAYABLE_OR);
                builder.Property(e => e.CL_PP_NO);
                builder.Property(e => e.CL_PROD);
                builder.Property(e => e.CL_PROVIDER_NO);
                builder.Property(e => e.CL_PROVIDER_TYPE);
                builder.Property(e => e.CL_PROVNAME);
                builder.Property(e => e.CL_RISK);
                builder.Property(e => e.CL_SEQID);
                builder.Property(e => e.CL_SRVC);
                builder.Property(e => e.CL_STATUS);
                builder.Property(e => e.CL_STLDATE);
                builder.Property(e => e.CL_STSER);
                builder.Property(e => e.CL_SUBADM);
                builder.Property(e => e.CL_SUBOFF);
                builder.Property(e => e.CL_SUBRISK);
                builder.Property(e => e.CL_SYSDATE);
                builder.Property(e => e.CL_TRFTUI);
                builder.Property(e => e.CL_VATAMT);
                builder.Property(e => e.CL_VATNET);
                builder.Property(e => e.CL_VISA);
                builder.Property(e => e.CL_VISA_CODE);
                builder.Property(e => e.CL_VISA_STS);
                builder.Property(e => e.Code);
                builder.Property(e => e.CONGINATAL_CHK);
                builder.Property(e => e.EMERGENCY_CHK);
                builder.Property(e => e.FROM_DATE);
                builder.Property(e => e.GROSS_LL);
                builder.Property(e => e.GROSS_OR);
                builder.Property(e => e.Id);
                builder.Property(e => e.InsPolicyNo);
                builder.Property(e => e.LOGIN_DATE);
                builder.Property(e => e.NationalID);
                builder.Property(e => e.NETPAYABLE_LL);
                builder.Property(e => e.NETPAYABLE_OR);
                builder.Property(e => e.POLICY_INC);
                builder.Property(e => e.POLICY_SEQ);
                builder.Property(e => e.PRE_DISEASE_CHK);
                builder.Property(e => e.PROVIDER_CITY);
                builder.Property(e => e.SERIAL);
                builder.Property(e => e.SRV_DESC);
                builder.Property(e => e.STATUS);
                builder.Property(e => e.TO_DATE);
                builder.Property(e => e.YearofBirth);
            });
            //ReimbursmentClaims
            modelBuilder.Entity<MRRequest>(builder =>
            {
                builder.Property(e => e.ActualAmount);
                builder.Property(e => e.CardExpireDate);
                builder.Property(e => e.CardNumber);
                builder.Property(e => e.ClaimTypeName);
                builder.Property(e => e.ClientId);         
                builder.Property(e => e.ExpectedAmount);
                builder.Property(e => e.HolderName);
               // builder.Property(e => e.Id);
                builder.Property(e => e.MemberID);
                builder.Property(e => e.MemberName);
                builder.Property(e => e.PolicyNumber);
                builder.Property(e => e.RelationName);
                builder.Property(e => e.RequestDate);             
                builder.Property(e => e.RequestNumber);
                builder.Property(e => e.RequestStatusId);         
                builder.Property(e => e.TransferDate);
                builder.Property(e => e.VATAmount);
            });  
            
            //ClientDTO
            modelBuilder.Entity<MRClient>(builder =>
            {
                builder.Property(e => e.BankName);
               
                builder.Property(e => e.ClientName);
                builder.Property(e => e.Email);
                builder.Property(e => e.GenderId);
                builder.Property(e => e.IBANNumber);
                builder.Property(e => e.Id);
                builder.Property(e => e.IDNumber);
                builder.Property(e => e.MobileNumber);
            });
            //RequestFile
            modelBuilder.Entity<RequestFileDTO>(builder =>
            {
                builder.Property(e => e.Comment);
                builder.Property(e => e.FileDesc);
                builder.Property(e => e.FileId);
                builder.Property(e => e.FileName);
                builder.Property(e => e.FilePath);
           
                builder.Property(e => e.MyFile);
            });

            modelBuilder.Entity<MRClaimType>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.ClaimTypeName);
               
            });
            modelBuilder.Entity<BankMaster>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.BankCode);
                builder.Property(e => e.BankNameArabic);
                builder.Property(e => e.BankNameEnglish);

            });

            modelBuilder.Entity<ClientDTO>(builder =>
            {
                builder.Property(e => e.BankName);

                builder.Property(e => e.ClientName);
                builder.Property(e => e.Email);
                builder.Property(e => e.GenderName);
                builder.Property(e => e.IBANNumber);
                builder.Property(e => e.Id);
                builder.Property(e => e.IDNumber);
                builder.Property(e => e.MobileNumber);
            });

            modelBuilder.Entity<TOB>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.PolicyNo);
                builder.Property(e => e.PolicyFromDate);
                builder.Property(e => e.PolicyToDate);
                builder.Property(e => e.ClassCode);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.Network);
            });

            modelBuilder.Entity<TOBlist>(builder =>
            {
                //builder.HasNoKey();
                builder.Property(e => e.Id);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.InsuranceClass);
                builder.Property(e => e.GeographicalLimit);
                builder.Property(e => e.Network);
                builder.Property(e => e.MemberAnnualLimit);
                builder.Property(e => e.PreExChrCondition);                
            });

            modelBuilder.Entity<Inpatient>(builder =>
            {
            
                builder.Property(e => e.ClassName);
                builder.Property(e => e.RoomType);
                builder.Property(e => e.RoomDayLimit);
                builder.Property(e => e.Accommodation);
                builder.Property(e => e.Surgical);
                builder.Property(e => e.MedicalNursing);
                builder.Property(e => e.Servicessupplies);
                builder.Property(e => e.IntensiveCareUnit);
                builder.Property(e => e.parentaccommodation);
                builder.Property(e => e.Co_insurance_Deductible);                
            });

            modelBuilder.Entity<Outpatient>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.Consultations);
                builder.Property(e => e.Diagnosticprocedures);
                builder.Property(e => e.Prescribeddrugs);
                builder.Property(e => e.Radiotherapy);
                builder.Property(e => e.Physiotherapy);
                builder.Property(e => e.MPN);
                builder.Property(e => e.OHN);
                builder.Property(e => e.OCN);               
            });

            modelBuilder.Entity<MaternityBenefit>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.Maternity_SubLimit);
                builder.Property(e => e.NormalDelivery);
                builder.Property(e => e.Caesarian);
                builder.Property(e => e.Miscarriage);
                builder.Property(e => e.WaitingPeriod);
                builder.Property(e => e.coverageofnewborns);
                builder.Property(e => e.PrematureBabies);
                builder.Property(e => e.MaternityInpatient);
            });

            modelBuilder.Entity<DentalBenefit>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.DentalConsultation);
                builder.Property(e => e.Extraction);
                builder.Property(e => e.X_Ray);
                builder.Property(e => e.Filling);
                builder.Property(e => e.RootCanalTreatment);
                builder.Property(e => e.GumInfectionTreatment);
                builder.Property(e => e.MaximumDentalSubLimit);
            });

            modelBuilder.Entity<ReimbursementClaim>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.EmgTreatmentOutside);
                builder.Property(e => e.NonEmgTreatmentOutside);
                builder.Property(e => e.EmgExpensesOutside);
                builder.Property(e => e.ConsGeneralPractitioner);
                builder.Property(e => e.Specialist_2);
                builder.Property(e => e.Specialist_1);
                builder.Property(e => e.Consultant);
                builder.Property(e => e.ConsultantforScarcity);
            });
           

            modelBuilder.Entity<AdditionalBenefit>(builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.ClassName);
                builder.Property(e => e.OpticalAids);
                builder.Property(e => e.HearingAids);
                builder.Property(e => e.KidneyDialysis);
                builder.Property(e => e.Acute_NonAcuteTreatment);
                builder.Property(e => e.Costsofacquiredheart);
                builder.Property(e => e.processoforgandonation);
                builder.Property(e => e.CostsofAlzheimer);
                builder.Property(e => e.Costsofautismcases);
                builder.Property(e => e.nationalprogramscreening);
                builder.Property(e => e.CostsofdisabilityCases);
                builder.Property(e => e.CongenitalDeformities);
                builder.Property(e => e.Circumcision);
                builder.Property(e => e.EarPiercing);
                builder.Property(e => e.LocAmbulanceCharges);
                builder.Property(e => e.mortalremainstoSaudi);
                builder.Property(e => e.coveringmorbidobesitytreatment);
            }); 
            
            
            modelBuilder.Entity<MRRequestStatusLog>(builder =>
            {
                builder.HasKey(e => e.StatusLogId);
                builder.Property(e => e.RequestId);
                builder.Property(e => e.RequestStatusId);
                builder.Property(e => e.Comment);
                builder.Property(e => e.ClientId);
                builder.Property(e => e.EntryEmpId);
                builder.Property(e => e.EntryDate);
            });

            modelBuilder.Entity<MRRequestFile>(builder =>
            {
                builder.HasKey(e => e.FileId);
                builder.Property(e => e.RequestId);
                builder.Property(e => e.FileDesc);
                builder.Property(e => e.FilePath);
                builder.Property(e => e.ClientId);
                builder.Property(e => e.EntryEmpId);
                builder.Property(e => e.EntryDate);
            });
        }
    }
}
