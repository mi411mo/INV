using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Utils.JSONValidationUtility;
using Domain.Models;
using Domain.Utils;
using Newtonsoft.Json;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceImpl
{
    public class InputValidationServiceImpl : InputValidationService
    {
        public async Task<string> Validate<T>(string jSONstring) where T : new()
        {
            RestAPIGenericResponseDTO<T> validationResponse = new RestAPIGenericResponseDTO<T>();
            List<ErrorModel> jsonValidationErrors = new List<ErrorModel>();

            if (string.IsNullOrWhiteSpace(jSONstring))
            {
                jsonValidationErrors.Add(new ErrorModel
                {
                    ErrorCode = Constants.CLIENT_ERROR_CODE,
                    DeveloperMessage = Constants.JSONStringWrongFormatingMessage,
                    UserMessage = Constants.GeneralUserErrorMessage,
                });
                return JSONHelper<RestAPIGenericResponseDTO<T>>.GetJSONStr(validationResponse.WithJSONRequestValidationError(jsonValidationErrors));
            }
            jSONstring = jSONstring.Trim();
            if ((jSONstring.StartsWith("{") && jSONstring.EndsWith("}")) || //For object
                (jSONstring.StartsWith("[") && jSONstring.EndsWith("]"))) //For array
            {
                try
                {

                    var shemaMdl = DtoModelSchemaGenerateor.GenerateSchema<T>();
                    var schema = await JsonSchema.FromJsonAsync(shemaMdl);

                    //var validator = new JsonSchemaValidator();
                    //var errors = validator.Validate(jSONstring, schema);
                    var errors = schema.Validate(jSONstring);
                    var success = errors.Count == 0 ? true : (errors.Count > 0 ? false : false);

                    if (!success)
                    {
                        foreach (var message in errors)
                        {
                            jsonValidationErrors.Add(new ErrorModel
                            {
                                ErrorCode = Constants.CLIENT_ERROR_CODE,
                                DeveloperMessage = message.ToString(),
                                UserMessage = Constants.GeneralUserErrorMessage,
                            });
                        }
                        return JSONHelper<RestAPIGenericResponseDTO<T>>.GetJSONStr(validationResponse.WithJSONRequestValidationError(jsonValidationErrors));
                    }
                    return string.Empty;
                }
                catch (JsonReaderException jex)
                {
                    jsonValidationErrors.Add(new ErrorModel
                    {
                        ErrorCode = Constants.CLIENT_ERROR_CODE,
                        DeveloperMessage = jex.Message.Split(" Path ").GetValue(0).ToString(),
                        UserMessage = Constants.GeneralUserErrorMessage,
                    });
                    return JSONHelper<RestAPIGenericResponseDTO<T>>.GetJSONStr(validationResponse.WithJSONRequestValidationError(jsonValidationErrors));
                }
                catch (Exception ex)
                {
                    jsonValidationErrors.Add(new ErrorModel
                    {
                        ErrorCode = Constants.CLIENT_ERROR_CODE,
                        DeveloperMessage = ex.Message.Split(" Path ").GetValue(0).ToString(),
                        UserMessage = Constants.GeneralUserErrorMessage,
                    });
                    return JSONHelper<RestAPIGenericResponseDTO<T>>.GetJSONStr(validationResponse.WithJSONRequestValidationError(jsonValidationErrors));
                }
            }
            else
            {
                jsonValidationErrors.Add(new ErrorModel
                {
                    ErrorCode = Constants.CLIENT_ERROR_CODE,
                    DeveloperMessage = Constants.JSONStringWrongFormatingMessage,
                    UserMessage = Constants.GeneralUserErrorMessage,
                });
                return JSONHelper<RestAPIGenericResponseDTO<T>>.GetJSONStr(validationResponse.WithJSONRequestValidationError(jsonValidationErrors));
            }
        }
    }
}
