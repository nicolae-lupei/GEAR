﻿using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Razor.BaseControllers;
using GR.Core.Razor.Helpers.Filters;
using GR.Identity.Abstractions.Helpers.Attributes;
using GR.Localization.Abstractions;
using GR.Localization.Abstractions.ViewModels.LocalizationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GR.Core.Helpers.Archiving;
using Newtonsoft.Json.Linq;

namespace GR.Localization.Api.Controllers
{
    /// <summary>
    /// This is a localization api 
    /// </summary>
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    [GearAuthorize(GearAuthenticationScheme.IdentityWithBearer)]
    [Route("api/[controller]/[action]")]
    [JsonApiExceptionFilter]
    public class LocalizationApiController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject service
        /// </summary>
        private readonly ILocalizationService _service;

        #endregion

        public LocalizationApiController(ILocalizationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all languages, authentication is not required
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<LanguageCreateViewModel>>))]
        public async Task<JsonResult> GetAllLanguages()
            => await JsonAsync(_service.GetAllLanguagesAsync());

        /// <summary>
        /// Add new language
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [Authorize(Roles = GlobalResources.Roles.ADMINISTRATOR)]
        public async Task<JsonResult> AddLanguage([Required] AddLanguageViewModel model)
            => await JsonAsync(_service.AddLanguageAsync(model));

        /// <summary>
        /// Import language pack
        /// </summary>
        /// <param name="language"></param>
        /// <param name="filePack"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [Authorize(Roles = GlobalResources.Roles.ADMINISTRATOR)]
        public async Task<JsonResult> ImportLanguagePack([Required]string language, [Required]IFormFile filePack)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();

            Dictionary<string, string> dict;
            using (var reader = new StreamReader(filePack.OpenReadStream()))
            {
                var content = await reader.ReadToEndAsync();
                dict = content.Deserialize<Dictionary<string, string>>();
            }

            var importResult = await _service.ImportLanguageTranslationsAsync(language, dict);
            return Json(importResult);
        }

        /// <summary>
        /// Get language pack
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLanguagePack([StringLength(2), Required]string language)
        {
            var request = await _service.GetLanguagePackAsync(language);
            if (!request.IsSuccess) return NotFound();
            var serialized = JObject.FromObject(request.Result).ToString(Formatting.Indented);
            return File(serialized.Utf8ToBytes(), ContentType.ApplicationJson, $"{language}.json");
        }

        /// <summary>
        /// Get language packs as archive
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLanguagePacks()
        {
            var request = await _service.GetLanguagePacksAsync();
            if (!request.IsSuccess) return NotFound();
            var items = new Dictionary<string, MemoryStream>();
            foreach (var languagePack in request.Result)
            {
                var serialized = JObject.FromObject(languagePack.Value).ToString(Formatting.Indented);
                items.Add($"{languagePack.Key}.json", new MemoryStream(serialized.Utf8ToBytes()));
            }
            var archive = ZipHelper.CreateZipArchive(items);
            return File(archive, ContentType.ApplicationZip, "LanguagePackArchive");
        }

        /// <summary>
        /// Get jquery data table translations
        /// </summary>
        /// <param name="language"></param>
        /// <param name="customReplace"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetJQueryTableTranslations(string language, string customReplace = null)
        {
            if (string.IsNullOrEmpty(language))
            {
                return new JsonResult(new object());
            }
            var link = $"http://cdn.datatables.net/plug-ins/1.10.19/i18n/{language}.json";
            using (var ctx = new WebClient())
            {
                try
                {
                    var jsonStr = ctx.DownloadString(link);
                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
                    if (string.IsNullOrEmpty(customReplace))
                        return new JsonResult(json);
                    try
                    {
                        var data = JsonConvert.DeserializeObject<IList<KeyValuePair<string, object>>>(customReplace);
                        if (!data.Any())
                            return new JsonResult(json);
                        foreach (var item in data)
                        {
                            if (json.ContainsKey(item.Key))
                            {
                                json[item.Key] = item.Value;
                            }
                        }
                        return new JsonResult(json);
                    }
                    catch
                    {
                        return new JsonResult(json);
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return new JsonResult(new object());
                }
            }
        }
    }
}
