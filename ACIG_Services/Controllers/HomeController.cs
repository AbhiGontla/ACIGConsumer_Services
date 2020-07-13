using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Api;
using Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Interfaces;


namespace ACIG_Services.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private ICustomerService _customerService;
        private readonly IOptions<ApplConfig> appSettings;

        public HomeController(IOptions<ApplConfig> _config, ICustomerService customerService)
        {
            appSettings = _config;
            _customerService = customerService;
        }

        [Route("GetApprovals")]
        [HttpPost]
        public async Task<GetApprovalsResponse> GetApprovals(ClsInput clsInput)
        {
            List<Approvals> _approvals = null;
            GetApprovalsResponse res = null;
            //check whether is user approvals in db or not
            _approvals = _customerService.GetApprovalsByNationalId(clsInput.nationalID);
            if (_approvals.Count > 0)
            {
                res = new GetApprovalsResponse();
                res.responseCode = "Success";
                res.responseData = _approvals;
                res.responseMessage = "User Approvals From Table";
                return res;
            }
            else
            {
                string url = appSettings.Value.Urls.GetApprovals;
                string username = appSettings.Value.BasicAuth.Username;
                string pass = appSettings.Value.BasicAuth.Password;

                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                //This is the key section you were missing    
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(username + ":" + pass);
                string val = System.Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                //Getting the input paramters as json 
                string content = GetJson(clsInput.nationalID, clsInput.yearOfBirth, clsInput.insPolicyNo);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<GetApprovalsResponse>(response.Content.ReadAsStringAsync().Result);
                    _approvals = res.responseData;
                    var result = GetApprovData(_approvals, clsInput);
                    try
                    {
                        _customerService.Insert(result);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                {
                    res.responseCode = response.StatusCode.ToString();
                    return res;
                }
                return res;
            }
        }

        #region GetInputJson    
        public string GetJson(string nationalId, string YOB, string insPolicyno)
        {
            string clientSecret = "{\r\n\"code\":\"CI\",\r\n\"nationalID\": \"" + nationalId + "\",\r\n\"yearOfBirth\": \"" + YOB + "\",\r\n\"insPolicyNo\": \"" + insPolicyno + "\"\r\n}\r\n";
            return clientSecret;
        }
        #endregion


        [Route("GetCustomers")]
        [HttpGet]
        public Registration GetCustomers()
        {
            var cust = _customerService.GetCustomerById("2332978820");
            return cust;
        }

        private static List<Approvals> GetApprovData(List<Approvals> approvals, ClsInput clsInput)
        {
            List<Approvals> _app;
            try
            {
                _app = approvals;
                foreach (var i in _app)
                {
                    i.Code = clsInput.code;
                    i.NationalId = clsInput.nationalID;
                    i.YearofBirth = clsInput.yearOfBirth;
                    i.InsPolicyNo = clsInput.insPolicyNo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _app;
        }

        [Route("GetPolicies")]
        [HttpPost]
        public PolicyResponse GetPoicies(ClsInput clsInput)
        {
            List<Policies> _policies = null;
            PolicyResponse res = null;
            //check whether is user approvals in db or not
            _policies = _customerService.GetPoiciesByNationalId(clsInput.nationalID);
            if (_policies.Count > 0)
            {
                res = new PolicyResponse();
                res.responseCode = "Success";
                res.responseData = _policies;
                res.responseMessage = "User Policies From Table";
                return res;
            }
            else
            {
                return res;
            }
        }



        [Route("GetCoverageBalances")]
        [HttpPost]
        public async Task<CoverageBalanceResponse> GetCoverageBalances(ClsInput clsInput)
        {
            List<CoverageBalance> coverageBalances = null;
            CoverageBalanceResponse res = null;
            //check whether is user covragebalances in db or not
            coverageBalances = _customerService.GetCovBalsByNationalId(clsInput.nationalID);
            if (coverageBalances.Count > 0)
            {
                res = new CoverageBalanceResponse();
                res.responseCode = "Success";
                res.responseData = coverageBalances;
                res.responseMessage = "User CoverageBalances From Table";
                return res;
            }
            else
            {
                string url = appSettings.Value.Urls.GetCoverageBalance;
                string username = appSettings.Value.BasicAuth.Username;
                string pass = appSettings.Value.BasicAuth.Password;

                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                //This is the key section you were missing    
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(username + ":" + pass);
                string val = System.Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                //Getting the input paramters as json 
                string content = GetJson(clsInput.nationalID, clsInput.yearOfBirth, clsInput.insPolicyNo);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<CoverageBalanceResponse>(response.Content.ReadAsStringAsync().Result);
                    coverageBalances = res.responseData;
                    var result = GetCovData(coverageBalances, clsInput);
                    try
                    {
                        _customerService.Insert(result);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                {
                    res.responseCode = response.StatusCode.ToString();
                    return res;
                }
                return res;
            }
        }
        private static List<CoverageBalance> GetCovData(List<CoverageBalance> coverageBalances, ClsInput clsInput)
        {
            List<CoverageBalance> _cov;
            try
            {
                _cov = coverageBalances;
                foreach (var i in _cov)
                {
                    i.NationalID = clsInput.nationalID;
                    i.YearofBirth = clsInput.yearOfBirth;
                    i.InsPolicyNo = clsInput.insPolicyNo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return _cov;
        }


        [Route("GetProvidersList")]
        [HttpPost]
        public async Task<ProvidersResponse> GetProviders(ClsInput clsInput)
        {
            List<Providers> providers = null;
            ProvidersResponse res = null;
            //check whether is user covragebalances in db or not
            providers = _customerService.GetProvidersByNationalId(clsInput.nationalID);
            if (providers.Count > 0)
            {
                res = new ProvidersResponse();
                res.responseCode = "Success";
                res.responseData = providers;
                res.responseMessage = "User Providers From Table";
                return res;
            }
            else
            {
                string url = appSettings.Value.Urls.GetProvidersList;
                string username = appSettings.Value.BasicAuth.Username;
                string pass = appSettings.Value.BasicAuth.Password;

                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                //This is the key section you were missing    
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(username + ":" + pass);
                string val = System.Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                //Getting the input paramters as json 
                string content = GetJson(clsInput.nationalID, clsInput.yearOfBirth, clsInput.insPolicyNo);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<ProvidersResponse>(response.Content.ReadAsStringAsync().Result);
                    providers = res.responseData;
                    var result = GetProvData(providers, clsInput);
                    try
                    {
                        _customerService.Insert(result);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                {
                    res.responseCode = response.StatusCode.ToString();
                    return res;
                }
                return res;
            }
        }
        private static List<Providers> GetProvData(List<Providers> Providers, ClsInput clsInput)
        {
            List<Providers> _prov;
            try
            {
                _prov = Providers;
                foreach (var i in _prov)
                {
                    i.Code = clsInput.code;
                    i.NationalID = clsInput.nationalID;
                    i.YearofBirth = clsInput.yearOfBirth;
                    i.InsPolicyNo = clsInput.insPolicyNo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return _prov;
        }

        [Route("GetPaidClaims")]
        [HttpPost]
        public async Task<PaidClaimsResponse> GetPaidClaims(ClsInput clsInput)
        {
            List<PaidClaims> paidClaims = null;
            PaidClaimsResponse res = null;
            //check whether is user covragebalances in db or not
            paidClaims = _customerService.GetPaidClaimsByNationalId(clsInput.nationalID);
            if (paidClaims.Count > 0)
            {
                res = new PaidClaimsResponse();
                res.responseCode = "Success";
                res.responseData = paidClaims;
                res.responseMessage = "User paidClaims From Table";
                return res;
            }
            else
            {
                string url = appSettings.Value.Urls.GetPaidClaims;
                string username = appSettings.Value.BasicAuth.Username;
                string pass = appSettings.Value.BasicAuth.Password;

                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                //This is the key section you were missing    
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(username + ":" + pass);
                string val = System.Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                //Getting the input paramters as json 
                string content = GetJson(clsInput.nationalID, clsInput.yearOfBirth, clsInput.insPolicyNo);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<PaidClaimsResponse>(response.Content.ReadAsStringAsync().Result);
                    paidClaims = res.responseData;
                    var result = GetPaidClaimsData(paidClaims, clsInput);
                    try
                    {
                        _customerService.Insert(result);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                {
                    res.responseCode = response.StatusCode.ToString();
                    return res;
                }
                return res;
            }
        }
        private static List<PaidClaims> GetPaidClaimsData(List<PaidClaims> paidclaims, ClsInput clsInput)
        {
            List<PaidClaims> _prov;
            try
            {
                _prov = paidclaims;
                foreach (var i in _prov)
                {
                    i.Code = clsInput.code;
                    i.NationalID = clsInput.nationalID;
                    i.YearofBirth = clsInput.yearOfBirth;
                    i.InsPolicyNo = clsInput.insPolicyNo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return _prov;
        }

        [Route("GetOSClaims")]
        [HttpPost]
        public async Task<OSClaimsResponse> GetOSClaims(ClsInput clsInput)
        {
            List<OSClaims> osClaims = null;
            OSClaimsResponse res = null;
            //check whether is user covragebalances in db or not
            osClaims = _customerService.GetOSClaimsByNationalId(clsInput.nationalID);
            if (osClaims.Count > 0)
            {
                res = new OSClaimsResponse();
                res.responseCode = "Success";
                res.responseData = osClaims;
                res.responseMessage = "User osclaims From Table";
                return res;
            }
            else
            {
                string url = appSettings.Value.Urls.GetOSClaims;
                string username = appSettings.Value.BasicAuth.Username;
                string pass = appSettings.Value.BasicAuth.Password;

                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                //This is the key section you were missing    
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(username + ":" + pass);
                string val = System.Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                //Getting the input paramters as json 
                string content = GetJson(clsInput.nationalID, clsInput.yearOfBirth, clsInput.insPolicyNo);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<OSClaimsResponse>(response.Content.ReadAsStringAsync().Result);
                    osClaims = res.responseData;
                    var result = GetOSClaimsData(osClaims, clsInput);
                    try
                    {
                        _customerService.Insert(result);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                {
                    res.responseCode = response.StatusCode.ToString();
                    return res;
                }
                return res;
            }
        }
        private static List<OSClaims> GetOSClaimsData(List<OSClaims> oSClaims, ClsInput clsInput)
        {
            List<OSClaims> _prov;
            try
            {
                _prov = oSClaims;
                foreach (var i in _prov)
                {
                    i.Code = clsInput.code;
                    i.NationalID = clsInput.nationalID;
                    i.YearofBirth = clsInput.yearOfBirth;
                    i.InsPolicyNo = clsInput.insPolicyNo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return _prov;
        }

        //[Route("GetReimbursmentClaims")]
        //[HttpPost]
        //public async Task<List<RequestCreateDTO>> GetReimbursmentClaims(ClsInput clsInput)
        //{
        //    List<RequestCreateDTO> requestCreateDTOs = null;
        //    ReimbursmentResponse res = null;           
        //    requestCreateDTOs = _customerService.GetreimClaimsByNationalId(clsInput.nationalID);
        //    if (requestCreateDTOs.Count > 0)
        //    {              

        //        return requestCreateDTOs;
        //    }
        //    else
        //    {
        //        HttpMessageHandler handler = new HttpClientHandler();
        //        string url = appSettings.Value.Urls.GetReimbursmentClaims;
        //        string cpath = url + clsInput.nationalID;
        //        var httpClient = new HttpClient(handler)
        //        {
        //            BaseAddress = new Uri(cpath),
        //            Timeout = new TimeSpan(0, 2, 0)
        //        };
        //        httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
        //        HttpResponseMessage response = await httpClient.GetAsync(cpath);
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            requestCreateDTOs = JsonConvert.DeserializeObject<RequestCreateDTO[]>(response.Content.ReadAsStringAsync().Result).ToList();

        //            try
        //            {
        //                _customerService.Insert(requestCreateDTOs);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        else
        //        {
        //            requestCreateDTOs = null;
        //            return requestCreateDTOs;
        //        }
        //    }

        //    return requestCreateDTOs;
        //}
        //[Route("GetReimbursmentClaimsById/id/{id}")]
        //[HttpGet]
        //public async Task<RequestCreateDTO> GetReimbursmentClaimsById(string id)
        //{
        //    RequestCreateDTO requestCreateDTOs = null;
        //    HttpMessageHandler handler = new HttpClientHandler();
        //    string url = appSettings.Value.Urls.GetReimbursmentDetails;
        //    string cpath = url + id;
        //    var httpClient = new HttpClient(handler)
        //    {
        //        BaseAddress = new Uri(cpath),
        //        Timeout = new TimeSpan(0, 2, 0)
        //    };
        //    httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
        //    HttpResponseMessage response = await httpClient.GetAsync(cpath);
        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        requestCreateDTOs = JsonConvert.DeserializeObject<RequestCreateDTO>(response.Content.ReadAsStringAsync().Result);

        //    }
        //    return requestCreateDTOs;
        //}

        [Route("AddClaimRequest")]
        [HttpPost]
        public async Task<string> InsertClaimRequest(RequestCreateDTO _claimdetails)
        {
            string Status = "false";
            string result;
            try
            {
                string url = appSettings.Value.Urls.AddReimbursmentClaims;
                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                //This is the key section you were missing    
                //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(username + ":" + pass);
                //string val = System.Convert.ToBase64String(plainTextBytes);
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                //Getting the input paramters as json 
                var content = JsonConvert.SerializeObject(_claimdetails);
              
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    result = null;
                }

                if (result != null)
                {
                    try
                    {
                        MRClient _list = _customerService.GetClientByNationalId(_claimdetails.NationalId);

                        if (_list == null)
                        {
                            MRClient _clientdet = new MRClient();
                            _clientdet.IDNumber = _claimdetails.NationalId;
                            _clientdet.BankName = _claimdetails.ClientDTO.BankName;
                            _clientdet.ClientName = _claimdetails.ClientDTO.ClientName;
                            _clientdet.Email = _claimdetails.ClientDTO.Email;
                            if (_claimdetails.ClientDTO.GenderName == null)
                            {
                                _clientdet.GenderId = null;
                            }
                            else if (_claimdetails.ClientDTO.GenderName.ToUpper() == "MALE")
                            {
                                _clientdet.GenderId = 1;
                            }
                            else if (_claimdetails.ClientDTO.GenderName.ToUpper() == "FEMALE")
                            {
                                _clientdet.GenderId = 2;
                            }
                            _clientdet.IBANNumber = _claimdetails.ClientDTO.IBANNumber;
                            _clientdet.MobileNumber = _claimdetails.ClientDTO.MobileNumber;

                            _customerService.Insert(_clientdet);
                        }

                        var _climdet = new MRRequest();
                        _climdet.ActualAmount = _claimdetails.ActualAmount;
                        _climdet.CardExpireDate = _claimdetails.CardExpireDate;
                        _climdet.CardNumber = _claimdetails.CardNumber;
                        _climdet.ClaimTypeName = _claimdetails.ClaimTypeName;
                        _climdet.RequestNumber = result;
                        _climdet.ClientId = _list.Id;
                        _climdet.ExpectedAmount = _claimdetails.ExpectedAmount;
                        _climdet.HolderName = _claimdetails.HolderName;
                        _climdet.MemberID = _claimdetails.MemberID;
                        _climdet.MemberName = _claimdetails.MemberName;
                        _climdet.PolicyNumber = _claimdetails.PolicyNumber;
                        _climdet.RelationName = _claimdetails.RelationName;
                        _climdet.RequestDate = _claimdetails.RequestDate;
                        _climdet.RequestStatusId = 1;
                        _climdet.TransferDate = _claimdetails.TransferDate;
                        _climdet.VATAmount = _claimdetails.VATAmount;

                        _customerService.Insert(_climdet);
                        Status = "true";

                    }
                    catch (Exception ex)
                    {
                        Status = "false";
                        throw ex;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            return Status;
        }

        [Route("GetClaimsByClientId/ClientId/{id}")]
        [HttpGet]
        public async Task<List<ReImClaims>> GetClaimsByClientId(string id)
        {
            List<ReImClaims> reImClaims = null;           
            try
            {
              
                HttpMessageHandler handler = new HttpClientHandler();
                string url = appSettings.Value.Urls.GetReimbursmentClaims;
                string cpath = url + id;
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(cpath),
                    Timeout = new TimeSpan(0, 2, 0)
                };
                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
                HttpResponseMessage response = await httpClient.GetAsync(cpath);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    
                    var a = JsonConvert.DeserializeObject<List<ReImClaims>>(response.Content.ReadAsStringAsync().Result);
                    reImClaims = a;
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }      
            return reImClaims;
        }


        [Route("GetClaimDetailsById/Id/{id}")]
        [HttpGet]
        public async Task<RequestCreateDTO> GetClaimDetailsById(string id)
        {
            RequestCreateDTO reImClaimdetails = null;
            try
            {

                HttpMessageHandler handler = new HttpClientHandler();
                string url = appSettings.Value.Urls.GetReimbursmentDetails;
                string cpath = url + id;
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(cpath),
                    Timeout = new TimeSpan(0, 2, 0)
                };
                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
                HttpResponseMessage response = await httpClient.GetAsync(cpath);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    var a = JsonConvert.DeserializeObject<RequestCreateDTO>(response.Content.ReadAsStringAsync().Result);
                    reImClaimdetails = a;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reImClaimdetails;
        }
    }
}
