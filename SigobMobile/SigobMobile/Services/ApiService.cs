﻿namespace SigobMobile.Services
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
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please turn on your phone internet settings.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "microsoft.com");
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

        //public async Task<TokenResponse> GetToken(
        //    string urlBase,
        //    string username,
        //    string password)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(urlBase);
        //        var response = await client.PostAsync("Token",
        //            new StringContent(string.Format(
        //            "grant_type=password&username={0}&password={1}",
        //            username, password),
        //            Encoding.UTF8, "application/x-www-form-urlencoded"));
        //        var resultJSON = await response.Content.ReadAsStringAsync();
        //        var result = JsonConvert.DeserializeObject<TokenResponse>(
        //            resultJSON);
        //        return result;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Get the specified objet (unsecure), calling and EndPoint through urlBase, servicePrefix, controller and identifier object.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="id">Identifier.</param>
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
            string token,
            string authToken,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(token, authToken);
                client.BaseAddress = new Uri(urlBase);
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
        /// <param name="token">Token type.</param>
        /// <param name="authToken">Access token.</param>
        /// <typeparam name="T">The object type parameter.</typeparam>
        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string token,
            string authToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(token, authToken);
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
        /// Post the specified urlBase, servicePrefix, controller, tokenType, accessToken and model.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="tokenType">Token type.</param>
        /// <param name="accessToken">Access token.</param>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request, Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
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
        /// Post the specified urlBase, servicePrefix, controller and model.
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
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
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
        /// Put the specified urlBase, servicePrefix, controller, tokenType, accessToken and model.
        /// </summary>
        /// <returns>The put.</returns>
        /// <param name="urlBase">URL base.</param>
        /// <param name="servicePrefix">Service prefix.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="tokenType">Token type.</param>
        /// <param name="accessToken">Access token.</param>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Response> Put<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    request,
                    Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controller,
                    model.GetHashCode());
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
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
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
    }
}
