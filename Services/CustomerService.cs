using Core;
using Core.Api;
using Core.Domain;
using Core.Domain.Customer;
using Data;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class CustomerService : ICustomerService
    {
        private IUnitOfWorks _unitOfWorks;

        public CustomerService(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }

        #region Policies

        #region GetPoliciesByNationalId
        public List<Policies> GetPoiciesByNationalId(string nationalID)
        {
            Registration _userdetails = GetCustomerById(nationalID);
            var res = _unitOfWorks.PoliciesRepository.GetDbSet();
            return res.Where(c => (c.PolicyNumber == _userdetails.PolicyNo) && (c.TushfaMemberNo == _userdetails.TushfaMemberNo)).ToList();
        }
        #endregion


        #region InsertPolicies
        public void Insert(Policies policies)
        {
            _unitOfWorks.PoliciesRepository.Insert(policies);
            _unitOfWorks.Save();
        } 
        #endregion

        #endregion

        public void Insert(Customer customer)
        {
            //_unitOfWorks.CustomerRepository.Insert(customer);
            //_unitOfWorks.Save();
        }



        #region Approvals

        #region InsertApprovals
        public void Insert(List<Approvals> approvals)
        {
            for (int i = 0; i < approvals.Count; i++)
            {
                _unitOfWorks.ApprovalsRepository.Insert(approvals[i]);
                _unitOfWorks.Save();
            }

        }
        #endregion

        #region GetApprovalsByNationalId
        public List<Approvals> GetApprovalsByNationalId(string NId)
        {
            var res = _unitOfWorks.ApprovalsRepository.GetDbSet();
            return res.Where(c => (c.NationalId == NId)).ToList();
        }
        #endregion

        #endregion

        #region Customer

        #region validateUser
        public Registration ValidateCustomer(string NId, string Pin)
        {
            var _customers = _unitOfWorks.RegistrationRepository.GetDbSet();
            return _customers.Where(c => (c.Iqama_NationalID == NId) && (c.ConfirmPin == Pin)).FirstOrDefault();
        }
        #endregion

        #region InsertUser
        public void Insert(Registration _userregistration)
        {
            try
            {
                _unitOfWorks.RegistrationRepository.Insert(_userregistration);
                _unitOfWorks.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetCustomerById
        public Registration GetCustomerById(string NId)
        {
            var _customers = _unitOfWorks.RegistrationRepository.GetDbSet();
            return _customers.Where(c => (c.Iqama_NationalID == NId)).FirstOrDefault();
        }


        #endregion

        #endregion


        #region CoverageBalances

        #region InsertCoverageBalances
        public void Insert(List<CoverageBalance> coverageBalances)
        {
            for (int i = 0; i < coverageBalances.Count; i++)
            {
                _unitOfWorks.CoverageBalanceRepository.Insert(coverageBalances[i]);
                _unitOfWorks.Save();
            }
        }

        #region GetCoverageBalanceById
        public List<CoverageBalance> GetCovBalsByNationalId(string NId)
        {
            var res = _unitOfWorks.CoverageBalanceRepository.GetDbSet();
            return res.Where(c => (c.NationalID == NId)).ToList();
        }
        #endregion
        #endregion

        #endregion


        #region Providers

        #region InsertProviders
        public void Insert(List<Providers> Providers)
        {
            for (int i = 0; i < Providers.Count; i++)
            {
                _unitOfWorks.ProvidersRepository.Insert(Providers[i]);
                _unitOfWorks.Save();
            }
        }

        #region GetProviderseById
        public List<Providers> GetProvidersByNationalId(string NId)
        {
            var res = _unitOfWorks.ProvidersRepository.GetDbSet();
            return res.Where(c => (c.NationalID == NId)).ToList();
        }


        #endregion
        #endregion

        #endregion

        #region PaidClaims
        public void Insert(List<PaidClaims> PaidClaims)
        {
            for (int i = 0; i < PaidClaims.Count; i++)
            {
                _unitOfWorks.PaidClaimsRepository.Insert(PaidClaims[i]);
                _unitOfWorks.Save();
            }
        }

        public List<PaidClaims> GetPaidClaimsByNationalId(string NId)
        {
            var res = _unitOfWorks.PaidClaimsRepository.GetDbSet();
            return res.Where(c => (c.NationalID == NId)).ToList();
        }
        #endregion

        #region OSClaims
        public void Insert(List<OSClaims> OSClaims)
        {
            for (int i = 0; i < OSClaims.Count; i++)
            {
                _unitOfWorks.OSClaimsRepository.Insert(OSClaims[i]);
                _unitOfWorks.Save();
            }
        }

        public List<OSClaims> GetOSClaimsByNationalId(string NId)
        {
            var res = _unitOfWorks.OSClaimsRepository.GetDbSet();
            return res.Where(c => (c.NationalID == NId)).ToList();
        }
        #endregion

        #region Reimbursmentclaims

        //public void Insert(MRClient client)
        //{
        //    try
        //    {
        //        _unitOfWorks.ClientRepository.Insert(client);
        //        _unitOfWorks.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public int Insert(MRClient client)
        {
            try
            {
                _unitOfWorks.ClientRepository.Insert(client);
                _unitOfWorks.Save();
                int id = client.Id;
                return id;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
      

        public MRClient GetClientByNationalId(string NId)
        {
            var res = _unitOfWorks.ClientRepository.GetDbSet();
            return res.Where(c => (c.IDNumber == NId)).FirstOrDefault();
        }

        public int Insert(MRRequest _reclaims)
        {
            _unitOfWorks.ReimbursmentRepository.Insert(_reclaims);
            _unitOfWorks.Save();
            int id = _reclaims.Id;
            return id;
        }

        //public void Insert(MRRequest _reclaims)
        //{
        //    _unitOfWorks.ReimbursmentRepository.Insert(_reclaims);
        //    _unitOfWorks.Save();
           
        //}


        public List<MRRequest> GetReimByNationalId(string NId)
        {
            var res = _unitOfWorks.ReimbursmentRepository.GetDbSet();
            return res.Where(c => (c.MemberID == NId)).ToList();
        }
              

        public List<MRRequest> GetreimClaimsByClientId(string id)
        {
            var _clientdetails = GetClientByNationalId(id);
            var res = _unitOfWorks.ReimbursmentRepository.GetDbSet();
            return res.Where(c => (c.MemberID == _clientdetails.IDNumber)).ToList();
        }

        public List<MRClaimType> GetClaimTypes()
        {
           return  _unitOfWorks.ClaimTypeRepository.GetDbSet().ToList();
        }

        public List<BankMaster> GetBankNames()
        {
            return _unitOfWorks.BankRepository.GetDbSet().ToList();
        }

        public List<Registration> GetAllCustomers()
        {
            return _unitOfWorks.RegistrationRepository.GetDbSet().ToList();
        }

        #endregion


        #region TOB

        public void Insert(TOB tOB)
        {
            _unitOfWorks.TOBRepository.Insert(tOB);
            _unitOfWorks.Save();
        }

        public TOB GetTOB(string PolicyNumber, string classcode)
        {
            var tobdetails = _unitOfWorks.TOBRepository.GetDbSet();
            return tobdetails.Where(c => (c.PolicyNo.ToString() == PolicyNumber)  &&(c.ClassCode == classcode)).FirstOrDefault();
        }

        public void Insert(TOBlist tOBlist)
        {
            _unitOfWorks.TOBListRepository.Insert(tOBlist);
            _unitOfWorks.Save();
        }

        public List<TOBlist> GetTOBList(string classname)
        {
            var tobdetails = _unitOfWorks.TOBListRepository.GetDbSet();
            return tobdetails.Where(c => (c.ClassName == classname)).ToList();
        }

        public void Insert(Inpatient inpatient)
        {
            _unitOfWorks.InpatientRepository.Insert(inpatient);
            _unitOfWorks.Save();
        }

        public List<Inpatient> GetInpatientList(string classname)
        {
            var tobdetails = _unitOfWorks.InpatientRepository.GetDbSet();
            return tobdetails.Where(c => (c.ClassName == classname)).ToList();
        }

        public void Insert(Outpatient outpatient)
        {
            _unitOfWorks.OutpatientRepository.Insert(outpatient);
            _unitOfWorks.Save();
        }

        public List<Outpatient> GetOutpatientList(string classname)
        {
            var tobdetails = _unitOfWorks.OutpatientRepository.GetDbSet();
            return tobdetails.Where(c => (c.ClassName == classname)).ToList();
        }

        public void Insert(MaternityBenefit maternityBenefit)
        {
            _unitOfWorks.MaternityBenefitRepository.Insert(maternityBenefit);
            _unitOfWorks.Save();
        }

        public List<MaternityBenefit> GetMaternityBenefitList(string classname)
        {
            var tobdetails = _unitOfWorks.MaternityBenefitRepository.GetDbSet();
            return tobdetails.Where(c => (c.ClassName == classname)).ToList();
        }

        public void Insert(DentalBenefit dentalBenefit)
        {
            _unitOfWorks.DentalBenefitRepository.Insert(dentalBenefit);
            _unitOfWorks.Save();
        }

        public List<DentalBenefit> GetDentalBenefitList(string classname)
        {
            var tobdetails = _unitOfWorks.DentalBenefitRepository.GetDbSet();
            return tobdetails.Where(c => (c.ClassName == classname)).ToList();
        }

        public void Insert(ReimbursementClaim reimbursementClaim)
        {
            _unitOfWorks.ReimbursementClaimRepository.Insert(reimbursementClaim);
            _unitOfWorks.Save();
        }

        public List<ReimbursementClaim> GetReimbursementClaimList(string classname)
        {
            var tobdetails = _unitOfWorks.ReimbursementClaimRepository.GetDbSet();
            return tobdetails.Where(c => (c.ClassName == classname)).ToList();
        }

        public void Insert(AdditionalBenefit additionalBenefit)
        {
            _unitOfWorks.AdditionalBenefitRepository.Insert(additionalBenefit);
            _unitOfWorks.Save();
        }

        public List<AdditionalBenefit> GetAdditionalBenefitList(string classname)
        {
            var tobdetails = _unitOfWorks.AdditionalBenefitRepository.GetDbSet();
            return tobdetails.Where(c => (c.ClassName == classname)).ToList();
        }


        #endregion


        public void Insert(MRRequestStatusLog mRRequestStatusLog)
        {
            _unitOfWorks.RequestStatusLogRepository.Insert(mRRequestStatusLog);
            _unitOfWorks.Save();
        }

        public void Insert(MRRequestFile requestFile)
        {
            _unitOfWorks.RequestFileRepository.Insert(requestFile);
            _unitOfWorks.Save();
        }

        public void UpdateRequestNumber(string requestid)
        {
            //var MRRequest = new MRRequest() { Id = Convert.ToInt32(requestId) };
            //_unitOfWorks.ReimbursmentRepository.Attach(MRRequest).Property(x => x.Password).IsModified = true;
            //db.SaveChanges();
        }
    }
}
