using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Api;
using Core.Domain;
using Core.Sms;
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
        private static string UploadedFilepath;
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


        [Route("GetCustomerById")]
        [HttpGet]
        public Registration GetCustomerById(string NationalId)
        {
            var cust = _customerService.GetCustomerById(NationalId);
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
                res = new PolicyResponse();
                res.responseCode = "Success";
                res.responseData = null;
                res.responseMessage = "User Policies Not Found";
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


        [Route("GetClaimsTypes")]
        [HttpGet]
        public List<MRClaimType> GetClaimsTypes()
        {
            try
            {
                return _customerService.GetClaimTypes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetBankNames")]
        [HttpGet]
        public List<BankMaster> GetBankNames()
        {
            try
            {
                return _customerService.GetBankNames();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("SendSms")]
        [HttpPost]
        public ResponseSMS SendSms(ClsSms clsSms)
        {

            int otp;
            string response = "";
            try
            {
                otp = GenerateRandomNo();
                string url = appSettings.Value.SmsConfig.url;
                string uname = appSettings.Value.SmsConfig.userName;
                string pwd = appSettings.Value.SmsConfig.password;
                string sender = appSettings.Value.SmsConfig.senderName;
                //string mobilenumber = "966508095931";
                string mobilenumber = clsSms.MobileNumber; ;
                string message = "Dear Customer,Your One Time Password(OTP):" + otp;
                SmsRequest request = new SmsRequest();
                response = request.SmsHandler(mobilenumber, message);

                if (response.ToString() == "Success")
                {
                    var responseSMS = new ResponseSMS();
                    responseSMS.RequestStatus = response;
                    responseSMS.ResponseText = "OTP Sent Successfully";
                    responseSMS.OTPSent = otp.ToString();
                    return responseSMS;
                }
                else
                {
                    var responseSMS = new ResponseSMS();
                    responseSMS.RequestStatus = response;
                    responseSMS.ResponseText = "OTP Sent Failed";
                    responseSMS.OTPSent = null;
                    return responseSMS;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Generate RandomNo
        public int GenerateRandomNo()
        {
            Random _rdm = new Random();
            int _min = 1000;
            int _max = 9999;
            return _rdm.Next(_min, _max);
        }

        [Route("GetAllUsers")]
        [HttpGet]
        public List<Registration> GetAllUsers()
        {
            return _customerService.GetAllCustomers();
        }


        [Route("UpdateClaim")]
        [HttpPost]
        public async Task<string> UpdateClaimRequest(UpdateClaimRequest updateClaim)
        {
            string status = "false";
            try
            {
                string url = appSettings.Value.Urls.UpdateClaimRequest;
                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");


                //Getting the input paramters as json 
                var content = JsonConvert.SerializeObject(updateClaim);

                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var st = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                    status = st;
                }
                else
                {
                    return status;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        [Route("RegistrationRequest")]
        [HttpPost]
        public async Task<Registration> RegistrationRequest(Registration registration)
        {
            //sample input-{"Iqama_NationalID":"1039640063","DOB":"01-05-1987"}
            string status = "false";
            Registration res = null;
            try
            {
                string url = appSettings.Value.Urls.RegistrationRequest;
                string username = appSettings.Value.BasicAuth.T_Username;
                string pass = appSettings.Value.BasicAuth.T_Password;
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
                var content = GetRegJson(registration.Iqama_NationalID, registration.DOB);

                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var st = JsonConvert.DeserializeObject<Registration>(response.Content.ReadAsStringAsync().Result);
                    status = "true";
                    if (st != null)
                    {
                        res = st;
                    }
                    else
                    {
                        res = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        #region GetRegJson    
        private string GetRegJson(string nationalId, string YOB)
        {
            string clientSecret = "{\"Id\":\"" + nationalId + "\",\"DOB\":\"" + YOB + "\"}";
            return clientSecret;
        }
        #endregion

        #region Policies

        #region GetAllPolicies
        [Route("GetAllPolicies")]
        [HttpPost]
        public async Task<PolicyResponse> GetAllPolicies(ClsInput clsInput)
        {
            List<Policies> _policies = null;
            PolicyResponse res = null;
            //check whether is user policies in db or not
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
                var customerDetails = GetAllUsers().Where(c => c.Iqama_NationalID == clsInput.nationalID).FirstOrDefault();
                res = await GetPolicyResponse(customerDetails.PolicyNo, customerDetails.TushfaMemberNo);
            }
            return res;
        }
        #endregion

        #region GetPolicyJson    
        private string GetPolicyJson(string PolicyNumber, string TushfaMemberNumber)
        {
            string clientSecret = "{\"PolicyNumber\":\"" + PolicyNumber + "\",\"TushfaMemberNumber\":\"" + TushfaMemberNumber + "\"}";
            return clientSecret;
        }
        #endregion

        #region GetPoliciesFromTPA
        private async Task<PolicyResponse> GetPolicyResponse(string policyno, string tushfamemno)
        {
            List<Policies> _policies = null;
            PolicyResponse res = null;
            try
            {
                string url = appSettings.Value.Urls.PoliciesRequest;
                string username = appSettings.Value.BasicAuth.T_Username;
                string pass = appSettings.Value.BasicAuth.T_Password;
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
                var content = GetPolicyJson(policyno, tushfamemno);

                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var st = JsonConvert.DeserializeObject<Policies>(response.Content.ReadAsStringAsync().Result);
                    if (st != null)
                    {
                        res = new PolicyResponse();
                        res.responseCode = "Success";
                        res.responseData = _policies;
                        res.responseMessage = "User Policies From Table";
                        _customerService.Insert(st);
                    }
                    else
                    {
                        res = new PolicyResponse();
                        res.responseCode = "Success";
                        res.responseData = null;
                        res.responseMessage = "User Policies Not Found";

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        #endregion

        #endregion

        #region TOB

        #region GetTOBs

        [Route("GetTOBs")]
        [HttpPost]
        public async Task<TOBResponse> GetTOBs(ClsInput clsInput)
        {

            TOB tOB = null;
            TOBResponse res = null;
            var customerDetails = GetCustomerById(clsInput.nationalID);
            tOB = _customerService.GetTOB(customerDetails.PolicyNo, customerDetails.ClassCode);
            if (tOB != null)
            {
                res = new TOBResponse();
                var tobDetails = GetTOBData(tOB.ClassName);
                tOB.TOBlist = tobDetails.TOBlist;
                tOB.TOBsublist = tobDetails.TOBsublist;
                res.responseCode = "Success";
                res.responseData = tOB;
                res.responseMessage = "User Policies From Table";
                return res;
            }
            else
            {
                res = await GetTOBResponse(customerDetails.PolicyNo, customerDetails.PolicyFromDate.ToString(), customerDetails.TushfaMemberNo);
                if (res != null)
                {
                    InsertTOB(res);
                }
            }
            return res;
        }
        #endregion

        #region InsertTOBinDB
        private bool InsertTOB(TOBResponse res)
        {
            bool status = false;
            try
            {
                if (res.responseData != null)
                {
                    TOB tob = new TOB();
                    tob.PolicyNo = res.responseData.PolicyNo;
                    tob.PolicyFromDate = res.responseData.PolicyFromDate;
                    tob.PolicyToDate = res.responseData.PolicyToDate;
                    tob.ClassCode = res.responseData.ClassCode;
                    tob.ClassName = res.responseData.ClassName;
                    tob.Network = res.responseData.Network;
                    _customerService.Insert(tob);

                    if (res.responseData.TOBlist.Count > 0)
                    {
                        for (int i = 0; i < res.responseData.TOBlist.Count; i++)
                        {
                            TOBlist toblist = new TOBlist();
                            toblist = res.responseData.TOBlist[i];
                            toblist.ClassName = res.responseData.ClassName;
                            _customerService.Insert(toblist);
                        }
                    }
                    if (res.responseData.TOBsublist.Count > 0)
                    {
                        for (int i = 0; i < res.responseData.TOBsublist.Count; i++)
                        {
                            if (res.responseData.TOBsublist[i].Inpatient.Count > 0)
                            {
                                for (int j = 0; j < res.responseData.TOBsublist[i].Inpatient.Count; j++)
                                {
                                    Inpatient inpatient = new Inpatient();
                                    inpatient = res.responseData.TOBsublist[j].Inpatient[j];
                                    inpatient.ClassName = res.responseData.ClassName;
                                    _customerService.Insert(inpatient);
                                }
                            }


                            if (res.responseData.TOBsublist[i].Outpatient.Count > 0)
                            {
                                for (int k = 0; k < res.responseData.TOBsublist[i].Outpatient.Count; k++)
                                {
                                    Outpatient outpatient = new Outpatient();
                                    outpatient = res.responseData.TOBsublist[k].Outpatient[k];
                                    outpatient.ClassName = res.responseData.ClassName;
                                    _customerService.Insert(outpatient);
                                }
                            }


                            if (res.responseData.TOBsublist[i].MaternityBenefits.Count > 0)
                            {
                                for (int j = 0; j < res.responseData.TOBsublist[i].MaternityBenefits.Count; j++)
                                {
                                    MaternityBenefit MaternityBenefit = new MaternityBenefit();
                                    MaternityBenefit = res.responseData.TOBsublist[j].MaternityBenefits[j];
                                    MaternityBenefit.ClassName = res.responseData.ClassName;
                                    _customerService.Insert(MaternityBenefit);
                                }
                            }


                            if (res.responseData.TOBsublist[i].Inpatient.Count > 0)
                            {
                                for (int j = 0; j < res.responseData.TOBsublist[i].DentalBenefits.Count; j++)
                                {
                                    DentalBenefit DentalBenefit = new DentalBenefit();
                                    DentalBenefit = res.responseData.TOBsublist[j].DentalBenefits[j];
                                    DentalBenefit.ClassName = res.responseData.ClassName;
                                    _customerService.Insert(DentalBenefit);
                                }
                            }


                            if (res.responseData.TOBsublist[i].ReimbursementClaims.Count > 0)
                            {
                                for (int j = 0; j < res.responseData.TOBsublist[i].ReimbursementClaims.Count; j++)
                                {
                                    ReimbursementClaim ReimbursementClaim = new ReimbursementClaim();
                                    ReimbursementClaim = res.responseData.TOBsublist[j].ReimbursementClaims[j];
                                    ReimbursementClaim.ClassName = res.responseData.ClassName;
                                    _customerService.Insert(ReimbursementClaim);
                                }
                            }


                            if (res.responseData.TOBsublist[i].AdditionalBenefits.Count > 0)
                            {
                                for (int j = 0; j < res.responseData.TOBsublist[i].AdditionalBenefits.Count; j++)
                                {
                                    AdditionalBenefit AdditionalBenefit = new AdditionalBenefit();
                                    AdditionalBenefit = res.responseData.TOBsublist[j].AdditionalBenefits[j];
                                    AdditionalBenefit.ClassName = res.responseData.ClassName;
                                    _customerService.Insert(AdditionalBenefit);
                                }
                            }

                        }
                    }

                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        #endregion

        #region GetTOBDetailsFromDB
        private TOB GetTOBData(string className)
        {
            TOB tOB = null;
            List<TOBlist> TOBlist = null;
            List<TOBsublist> TOBsub = null;
            if (className != null)
            {

                List<Inpatient> Inpatientlist;
                List<Outpatient> Outpatientlist;
                List<MaternityBenefit> MaternityBenefitslist;
                List<DentalBenefit> DentalBenefitslist;
                List<ReimbursementClaim> ReimbursementClaimslist;
                List<AdditionalBenefit> AdditionalBenefitslist;
                //assign values to TOB object
                tOB = new TOB();



                //gets the toblist data
                TOBlist = new List<TOBlist>();
                TOBlist = _customerService.GetTOBList(className);
                tOB.TOBlist = TOBlist;

                //gets the tobsublist data
                TOBsub = new List<TOBsublist>();
                TOBsublist sublist = new TOBsublist();
                AdditionalBenefitslist = _customerService.GetAdditionalBenefitList(className);
                sublist.AdditionalBenefits = AdditionalBenefitslist;
                DentalBenefitslist = _customerService.GetDentalBenefitList(className);
                sublist.DentalBenefits = DentalBenefitslist;
                Inpatientlist = _customerService.GetInpatientList(className);
                sublist.Inpatient = Inpatientlist;
                MaternityBenefitslist = _customerService.GetMaternityBenefitList(className);
                sublist.MaternityBenefits = MaternityBenefitslist;
                Outpatientlist = _customerService.GetOutpatientList(className);
                sublist.Outpatient = Outpatientlist;
                ReimbursementClaimslist = _customerService.GetReimbursementClaimList(className);
                sublist.ReimbursementClaims = ReimbursementClaimslist;
                TOBsub.Add(sublist);
                tOB.TOBsublist = TOBsub;
            }
            return tOB;
        }
        #endregion

        #region GetPolicyJson    
        private string GetTOBJson(string PolicyNumber, string PolicyFromDate, string ClassCode)
        {
            string clientSecret = "{\"PolicyNumber\":\"" + PolicyNumber + "\",\"PolicyFromDate\":\"" + PolicyFromDate + "\",\"ClassCode\":\"" + ClassCode + "\"}";
            return clientSecret;
        }
        #endregion

        #region GetTOBDetailsFromTPA
        private async Task<TOBResponse> GetTOBResponse(string PolicyNumber, string PolicyFromDate, string ClassCode)
        {
            TOBResponse res = null;
            try
            {
                string url = appSettings.Value.Urls.TOBRequest;
                string username = appSettings.Value.BasicAuth.T_Username;
                string pass = appSettings.Value.BasicAuth.T_Password;
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
                var content = GetTOBJson(PolicyNumber, PolicyFromDate, ClassCode);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var st = JsonConvert.DeserializeObject<TOB>(response.Content.ReadAsStringAsync().Result);
                    if (st != null)
                    {
                        res = new TOBResponse();
                        res.responseCode = "Success";
                        res.responseData = st;
                        res.responseMessage = "User Policies From TPA Server";
                        // _customerService.Insert(st);
                    }
                    else
                    {
                        res = new TOBResponse();
                        res.responseCode = "Success";
                        res.responseData = null;
                        res.responseMessage = "User Policies Not Found";

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        #endregion

        #endregion

        [Route("AddClaimRequest_New")]
        [HttpPost]
        public async Task<string> InsertClaimRequestNew(RequestCreateDTO _claimdetails)
        {
            string Status = "false";
            string result;
            string clientId = null;
            string requestId = null;
            try
            {
                MRClient _list = _customerService.GetClientByNationalId(_claimdetails.NationalId);
                clientId = _list.Id.ToString();
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

                    clientId = _customerService.Insert(_clientdet).ToString();                   
                }

                var _climdet = new MRRequest();
                _climdet.ActualAmount = _claimdetails.ActualAmount;
                _climdet.CardExpireDate = _claimdetails.CardExpireDate;
                _climdet.CardNumber = _claimdetails.CardNumber;
                _climdet.ClaimTypeName = _claimdetails.ClaimTypeName;                
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

                requestId=_customerService.Insert(_climdet).ToString();

                InsertCommentsFiles(clientId, requestId, _claimdetails);
                Status = "true";

            }
            catch (Exception ex)
            {
                Status = "false";               
            }


            if (Status == "true")
            {
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
                    //Getting the input paramters as json 
                    var content = JsonConvert.SerializeObject(_claimdetails);

                    var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);

                        _customerService.UpdateRequestNumber(requestId);
                    }
                    else
                    {
                        result = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
            return Status;
        }

        private void InsertCommentsFiles(string clientId, string requestId, RequestCreateDTO claimdetails)
        {
            if(claimdetails!=null && clientId!=null && requestId != null)
            {
                try
                {
                    MRRequestStatusLog rRequestStatusLog = new MRRequestStatusLog();
                    rRequestStatusLog.ClientId = Convert.ToInt32(clientId);
                    rRequestStatusLog.RequestId = Convert.ToInt32(requestId);
                    rRequestStatusLog.RequestStatusId = 1;
                    rRequestStatusLog.Comment = claimdetails.Comment;
                    rRequestStatusLog.EntryDate = DateTime.Now;
                    rRequestStatusLog.EntryEmpId = 0;
                    _customerService.Insert(rRequestStatusLog);

                    foreach(var item in claimdetails.RequestFileList)
                    {
                        MRRequestFile requestFile = new MRRequestFile();
                        requestFile.RequestId = Convert.ToInt32(requestId);
                        requestFile.FileDesc = item.FileDesc;
                        SaveFile(item);
                        requestFile.FilePath = UploadedFilepath;
                        requestFile.ClientId = Convert.ToInt32(clientId);
                        requestFile.EntryDate = DateTime.Now;
                        requestFile.IsClientVisible = true;
                        requestFile.EntryEmpId = 0;
                        requestFile.IsActive = true;
                        _customerService.Insert(requestFile);
                    }

                }
                catch(Exception ex)
                {

                }
            }
          

        }

        private async void SaveFile(RequestFileDTO item)
        {
            string uploadPath = @"E:/Uploads/";
            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(item.FileDesc);
            var uploadPathWithfileName = Path.Combine(uploadPath, fileName);

          

            using (var fileStream = new FileStream(uploadPathWithfileName, FileMode.Create))
            {
                byte[] filest = item.MyFile;
                var stream = new MemoryStream(filest);
                IFormFile file = new FormFile(stream, 0, filest.Length, "name", item.FileDesc);
                await file.CopyToAsync(fileStream);
                UploadedFilepath = uploadPathWithfileName;
            }
        }
    }
}
