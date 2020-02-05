namespace SigobMobile.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    //using Domain;

    public class ApiService
    {
        /// <summary>
        /// Checks the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please turn on your internet settings.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Check you internet connection.",
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }

        #region GET API Calls

        /// <summary>
        /// Get the specified objet (unsecure), calling and EndPoint through urlBase, servicePrefix, controller and identifier object.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="id">Identifier. if value = 0 noy use id in api call</param>
        /// <typeparam name="T">The object type parameter.</typeparam>
        public async Task<Response> Get<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                var url = (id<0) ? $"{servicePrefix}{controller}" : $"{servicePrefix}{controller}/{id}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// Get the specified object, calling an EndPoint through urlBase, servicePrefix, controller, tokenType and accessToken.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="authToken">Authorized Token.</param>
        /// <param name="authDbToken">Access database token.</param>
        /// <typeparam name="T">The object type parameter.</typeparam>
        public async Task<Response> Get<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string authToken,
            string authDbToken)
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(urlBase)
            };
            try
            {
                client.DefaultRequestHeaders.Add("token", authToken);
                client.DefaultRequestHeaders.Add("dbtoken", authDbToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally { client.Dispose(); }
        }


        /// <summary>
        /// Get the specified object, calling an EndPoint through urlBase, servicePrefix, controller, tokenType, accessToken and id.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="token">Token type.</param>
        /// <param name="authToken">Access token.</param>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The object type parameter.</typeparam>
        public async Task<Response> Get<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string authToken,
            string authDbToken,
            int id)
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(urlBase)
            };
            try
            {
                client.DefaultRequestHeaders.Add("token", authToken);
                client.DefaultRequestHeaders.Add("dbtoken", authDbToken);
                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally { client.Dispose(); }
        }

        /// <summary>
        /// Gets list without token (unsecured method).
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller name</param>
        /// <typeparam name="T">The list type (any object).</typeparam>
        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                var client = new HttpClient()
                {
                    BaseAddress = new Uri(urlBase)
                };
                var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// Gets the list (secured method)
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="authToken">Authorization token.</param>
        /// <param name="authDbToken">Access database token.</param>
        /// <typeparam name="T">The object type parameter.</typeparam>
        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string authToken,
            string authDbToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                client.DefaultRequestHeaders.Add("token", authToken);
                client.DefaultRequestHeaders.Add("dbtoken", authDbToken);
                //client.DefaultRequestHeaders.Authorization. =
                //new AuthenticationHeaderValue("dbtoken", dbToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        #endregion


        #region POST API Calls

        /// <summary>
        /// Post without body content, specified urlBase, servicePrefix, controller, authToken and authDbToken.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="authToken">Auth token.</param>
        /// <param name="authDbToken">Auth db token.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string authToken,
            string authDbToken)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                client.DefaultRequestHeaders.Add("token", authToken);
                client.DefaultRequestHeaders.Add("dbtoken", authDbToken);
                //client.DefaultRequestHeaders.Authorization =
                //new AuthenticationHeaderValue(tokenType, accessToken);

                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, null);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// Post the specified urlBase, servicePrefix, controller, tokenType, accessToken and model.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="authToken">Authorization token.</param>
        /// <param name="authDbToken">Access token.</param>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Response> Post<T>(
        string urlBase,
        string servicePrefix,
        string controller,
        string authToken,
        string authDbToken,
        T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request, Encoding.UTF8,
                    "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                client.DefaultRequestHeaders.Add("token", authToken);
                client.DefaultRequestHeaders.Add("dbtoken", authDbToken);
                //client.DefaultRequestHeaders.Authorization =
                //new AuthenticationHeaderValue(tokenType, accessToken);

                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// Unsecured Post the specified urlBase, servicePrefix, controller and model.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// Login Post the specified urlBase, servicePrefix, controller and model.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Response> PostLogin<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<SessionSigob>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Success login!",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        #endregion

        #region PUT API Call
        /// <summary>
        /// Put without Model the specified urlBase, servicePrefix, controller, authToken and authDbToken.
        /// </summary>
        /// <returns>The put.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="authToken">Auth token.</param>
        /// <param name="authDbToken">Auth db token.</param>
        public async Task<Response> Put<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string authToken,
            string authDbToken)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                client.DefaultRequestHeaders.Add("token", authToken);
                client.DefaultRequestHeaders.Add("dbtoken", authDbToken);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PutAsync(url, null);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// Put the specified urlBase, servicePrefix, controller, tokenType, accessToken and model.
        /// </summary>
        /// <returns>The put.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="authToken">Auth token.</param>
        /// <param name="authDbToken">Auth db token.</param>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Response> Put<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string authToken,
            string authDbToken,
            object model)
        {
            HttpClient client = null;
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8, "application/json");
                //HttpClient
                client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                client.DefaultRequestHeaders.Add("token", authToken);
                client.DefaultRequestHeaders.Add("dbtoken", authDbToken);
                var url = $"{servicePrefix}{controller}"; //model.GetHashCode());
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally { client.Dispose(); }
        }

        #endregion

        #region DELETE API Call

        /// <summary>
        /// Delete the specified urlBase, servicePrefix, controller, tokenType, accessToken.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="tokenType">Token type.</param>
        /// <param name="accessToken">Access token.</param>
        /// <param name="id">Object id to delete.</param>
        public async Task<Response> DeleteAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string authToken,
            string authDbToken,
            int id)
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(urlBase)
            };
            try
            {
                client.DefaultRequestHeaders.Add("token", authToken);
                client.DefaultRequestHeaders.Add("dbtoken", authDbToken);
                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally { client.Dispose(); }
        }

        /// <summary>
        /// Delete the specified urlBase, servicePrefix, controller, tokenType, accessToken and model.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="tokenType">Token type.</param>
        /// <param name="accessToken">Access token.</param>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Response> Delete<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
                var response = await client.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        #endregion
    }
}
