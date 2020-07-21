using Core;
using Core.Api;
using Core.Domain;
using Core.Domain.Customer;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class UnitOfWorks : IUnitOfWorks
    {

        private readonly ACIGDbContext _context;

        public UnitOfWorks(ACIGDbContext context)
        {
            _context = context;
        }
        

        bool disposed = false;

        private IRepository<Customer> customerRepository;
        private IRepository<Registration> registrationRepository;
       
        public IRepository<Customer> CustomerRepository
        {
            get
            {
                if (this.customerRepository == null)
                    this.customerRepository = new Repository<Customer>(_context);
                return customerRepository;
            }

        }
        public IRepository<Registration> RegistrationRepository
        {
            get
            {
                if (this.registrationRepository == null)
                    this.registrationRepository = new Repository<Registration>(_context);
                return registrationRepository;
            }

        }
        private IRepository<Approvals> approvalsRepository;
        public IRepository<Approvals> ApprovalsRepository
        {
            get
            {
                if (this.approvalsRepository == null)
                    this.approvalsRepository = new Repository<Approvals>(_context);
                return approvalsRepository;
            }

        } 
        private IRepository<Policies> policiesRepository;
        public IRepository<Policies> PoliciesRepository
        {
            get
            {
                if (this.policiesRepository == null)
                    this.policiesRepository = new Repository<Policies>(_context);
                return policiesRepository;
            }

        }
        private IRepository<CoverageBalance> coverageBalanceRepository;
        public IRepository<CoverageBalance> CoverageBalanceRepository
        {
            get
            {
                if (this.coverageBalanceRepository == null)
                    this.coverageBalanceRepository = new Repository<CoverageBalance>(_context);
                return coverageBalanceRepository;
            }

        }
        
        private IRepository<Providers> providersRepository;
        public IRepository<Providers> ProvidersRepository
        {
            get
            {
                if (this.providersRepository == null)
                    this.providersRepository = new Repository<Providers>(_context);
                return providersRepository;
            }

        }

        private IRepository<PaidClaims> paidClaimsRepository;
        public IRepository<PaidClaims> PaidClaimsRepository
        {
            get
            {
                if (this.paidClaimsRepository == null)
                    this.paidClaimsRepository = new Repository<PaidClaims>(_context);
                return paidClaimsRepository;
            }

        }
        private IRepository<OSClaims> osClaimsRepository;
        public IRepository<OSClaims> OSClaimsRepository
        {
            get
            {
                if (this.osClaimsRepository == null)
                    this.osClaimsRepository = new Repository<OSClaims>(_context);
                return osClaimsRepository;
            }

        }

        private IRepository<MRRequest> reimbursmentRepository;
        public IRepository<MRRequest> ReimbursmentRepository
        {
            get
            {
                if (this.reimbursmentRepository == null)
                    this.reimbursmentRepository = new Repository<MRRequest>(_context);
                return reimbursmentRepository;
            }

        }

        private IRepository<MRClient> clientRepository;
        public IRepository<MRClient> ClientRepository
        {
            get
            {
                if (this.clientRepository == null)
                    this.clientRepository = new Repository<MRClient>(_context);
                return clientRepository;
            }

        }

        private IRepository<MRRequestStatusLog> requestStatusLogRepository;
        public IRepository<MRRequestStatusLog> RequestStatusLogRepository
        {
            get
            {
                if (this.requestStatusLogRepository == null)
                    this.requestStatusLogRepository = new Repository<MRRequestStatusLog>(_context);
                return requestStatusLogRepository;
            }

        }
        private IRepository<MRRequestFile> requestFileRepository;
        public IRepository<MRRequestFile> RequestFileRepository
        {
            get
            {
                if (this.requestFileRepository == null)
                    this.requestFileRepository = new Repository<MRRequestFile>(_context);
                return requestFileRepository;
            }

        }

        private IRepository<MRClaimType> claimTypeRepository;
        public IRepository<MRClaimType> ClaimTypeRepository
        {
            get
            {
                if (this.claimTypeRepository == null)
                    this.claimTypeRepository = new Repository<MRClaimType>(_context);
                return claimTypeRepository;
            }

        }
        private IRepository<BankMaster> bankRepository;
        public IRepository<BankMaster> BankRepository
        {
            get
            {
                if (this.bankRepository == null)
                    this.bankRepository = new Repository<BankMaster>(_context);
                return bankRepository;
            }

        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityEntry = _context.Entry(entity);
            if (entityEntry == null)
                return;

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        #region TOB

        private IRepository<TOBlist> tobListRepository;
        public IRepository<TOBlist> TOBListRepository
        {
            get
            {
                if (this.tobListRepository == null)
                    this.tobListRepository = new Repository<TOBlist>(_context);
                return tobListRepository;
            }

        }
        private IRepository<TOB> tobRepository;
        public IRepository<TOB> TOBRepository
        {
            get
            {
                if (this.tobRepository == null)
                    this.tobRepository = new Repository<TOB>(_context);
                return tobRepository;
            }

        }
        private IRepository<Inpatient> inpatientRepository;
        public IRepository<Inpatient> InpatientRepository
        {
            get
            {
                if (this.inpatientRepository == null)
                    this.inpatientRepository = new Repository<Inpatient>(_context);
                return inpatientRepository;
            }

        }
        private IRepository<Outpatient> outpatientRepository;
        public IRepository<Outpatient> OutpatientRepository
        {
            get
            {
                if (this.outpatientRepository == null)
                    this.outpatientRepository = new Repository<Outpatient>(_context);
                return outpatientRepository;
            }

        }
        private IRepository<MaternityBenefit> maternityBenefitRepository;
        public IRepository<MaternityBenefit> MaternityBenefitRepository
        {
            get
            {
                if (this.maternityBenefitRepository == null)
                    this.maternityBenefitRepository = new Repository<MaternityBenefit>(_context);
                return maternityBenefitRepository;
            }

        }
        private IRepository<DentalBenefit> dentalBenefitRepository;
        public IRepository<DentalBenefit> DentalBenefitRepository
        {
            get
            {
                if (this.dentalBenefitRepository == null)
                    this.dentalBenefitRepository = new Repository<DentalBenefit>(_context);
                return dentalBenefitRepository;
            }

        }
        private IRepository<ReimbursementClaim> reimbursementClaimRepository;
        public IRepository<ReimbursementClaim> ReimbursementClaimRepository
        {
            get
            {
                if (this.reimbursementClaimRepository == null)
                    this.reimbursementClaimRepository = new Repository<ReimbursementClaim>(_context);
                return reimbursementClaimRepository;
            }

        }
        private IRepository<AdditionalBenefit> additionalBenefitRepository;
        public IRepository<AdditionalBenefit> AdditionalBenefitRepository
        {
            get
            {
                if (this.additionalBenefitRepository == null)
                    this.additionalBenefitRepository = new Repository<AdditionalBenefit>(_context);
                return additionalBenefitRepository;
            }

        }

        #endregion
    }
}
