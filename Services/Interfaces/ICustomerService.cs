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
        List<Policies> GetPoiciesByNationalId(string nationalID);

        public void Insert(List<Providers> Providers);
        public List<Providers> GetProvidersByNationalId(string NId);

        public void Insert(List<PaidClaims> PaidClaims);
        public List<PaidClaims> GetPaidClaimsByNationalId(string NId);

        public void Insert(List<OSClaims> OSClaims);
        public List<OSClaims> GetOSClaimsByNationalId(string NId);       

        

        public void Insert(MRClient _client);
        public MRClient GetClientByNationalId(string NId);

        public void Insert(MRRequest _claimreq);
        List<MRRequest> GetreimClaimsByClientId(string id);
    }
}
