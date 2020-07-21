using Core;
using Core.Api;
using Core.Domain;
using Core.Domain.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface ICustomerService
    {
        public void Insert(Customer customer);
        public Registration ValidateCustomer(string NId,string Pin);
        public Registration GetCustomerById(string NId);
        public void Insert(Registration _userregistration);
        public void Insert(List<Approvals> approvals);
        public void Insert(List<CoverageBalance> coverageBalances);
        public List<CoverageBalance> GetCovBalsByNationalId(string NId);
        public List<Approvals> GetApprovalsByNationalId(string NId);

        #region policies
        public void Insert(Policies policies);
        List<Policies> GetPoiciesByNationalId(string nationalID); 
        #endregion

        public void Insert(List<Providers> Providers);
        public List<Providers> GetProvidersByNationalId(string NId);

        public void Insert(List<PaidClaims> PaidClaims);
        public List<PaidClaims> GetPaidClaimsByNationalId(string NId);

        public void Insert(List<OSClaims> OSClaims);
        public List<OSClaims> GetOSClaimsByNationalId(string NId);


        //public void Insert(MRClient _client);
        public int Insert(MRClient _client);
        public MRClient GetClientByNationalId(string NId);

        public int Insert(MRRequest _claimreq);
        //public void Insert(MRRequest _claimreq);
        public List<MRRequest> GetreimClaimsByClientId(string id);

        public List<MRClaimType> GetClaimTypes();
        public List<BankMaster> GetBankNames();

        public void Insert(MRRequestStatusLog mRRequestStatusLog);
        public void Insert(MRRequestFile requestFile);

        public List<Registration> GetAllCustomers();

        public void UpdateRequestNumber(string requestid);

        #region TOB
        public void Insert(TOB tOB);

        public TOB GetTOB(string PolicyNumber,string classcode);
        public void Insert(TOBlist tOBlist);
        public List<TOBlist> GetTOBList(string classname);
        public void Insert(Inpatient inpatient);
        public List<Inpatient> GetInpatientList(string classname);
        public void Insert(Outpatient outpatient);
        public List<Outpatient> GetOutpatientList(string classname);
        public void Insert(MaternityBenefit maternityBenefit);
        public List<MaternityBenefit> GetMaternityBenefitList(string classname);
        public void Insert(DentalBenefit dentalBenefit);
        public List<DentalBenefit> GetDentalBenefitList(string classname);
        public void Insert(ReimbursementClaim reimbursementClaim);
        public List<ReimbursementClaim> GetReimbursementClaimList(string classname);
        public void Insert(AdditionalBenefit additionalBenefit);
        public List<AdditionalBenefit> GetAdditionalBenefitList(string classname);

        #endregion

    }
}
