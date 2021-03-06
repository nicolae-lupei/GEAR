﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GR.Core.Abstractions;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.ECommerce.Abstractions;
using GR.ECommerce.Abstractions.Extensions;
using GR.ECommerce.Abstractions.Helpers;
using GR.ECommerce.Abstractions.Models;
using GR.ECommerce.Razor.Helpers.BaseControllers;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using GR.Core;
using GR.Core.Razor.Extensions;
using GR.ECommerce.Abstractions.Models.Currencies;
using GR.ECommerce.Abstractions.ViewModels.ProductViewModels;
using GR.ECommerce.Razor.Helpers;
using GR.ECommerce.Razor.Models;
using GR.ECommerce.Razor.ViewModels.ProductsGalleryViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GR.ECommerce.Razor.Controllers
{
    [Authorize]
    public class ProductsController : CommerceBaseController<Product, ProductViewModel>
    {
        #region Injectable

        /// <summary>
        /// Inject product service
        /// </summary>
        private readonly IProductService<Product> _productService;

        /// <summary>
        /// Inject gallery manager
        /// </summary>
        private readonly ProductGalleryManager _galleryManager;
        #endregion

        public ProductsController(ICommerceContext context, IDataFilter dataFilter, IProductService<Product> productService, ProductGalleryManager galleryManager) : base(context, dataFilter)
        {
            _productService = productService;
            _galleryManager = galleryManager;
        }


        #region Views

        /// <inheritdoc />
        /// <summary>
        /// Index page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Dashboard
        /// </summary>
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        /// <summary>
        /// Index page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ProductDetail(Guid? productId)
        {
            if (productId == null) return NotFound();

            var productBd = await Context.Products
                .Include(i => i.ProductPrices)
                .Include(i => i.ProductImages)
                .Include(i => i.ProductAttributes)
                .ThenInclude(i => i.ProductAttribute)
                .Include(i => i.ProductVariations)
                .ThenInclude(i => i.ProductVariationDetails)
                .ThenInclude(i => i.ProductOption)
                .FirstOrDefaultAsync(x => x.Id == productId);

            if (productBd is null) return NotFound();

            var result = productBd.Adapt<ProductViewModel>();
            result.ProductOption = GetProdOptionByVariation(result.Id);
            result.ProductVariationList = GetProdVariationList(result.Id);

            return View(result);
        }

        /// <inheritdoc />
        /// <summary>
        /// Create new product
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IActionResult Create()
        {
            var result = new ProductViewModel();
            return View(GetDropdownItems(result));
        }

        /// <inheritdoc />
        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<IActionResult> Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddCommerceError(CommerceErrorKeys.InvalidModel);
                return View(model);
            }

            await Context.Products.AddAsync(model);
            await Context.ProductPrices.AddAsync(new ProductPrice
            {
                Product = model,
                Price = model.Price
            });
            var dbResult = await Context.PushAsync();

            if (dbResult.IsSuccess)
            {
                if (model.ProductImagesList != null && model.ProductImagesList.Any())
                {
                    await _galleryManager.AddImagesOnCreateAsync(model.Id, model.ProductImagesList);
                }
                return RedirectToAction(nameof(Index));
            }

            ModelState.AppendResultModelErrors(dbResult.Errors);

            return View(model);
        }

        /// <inheritdoc />
        /// <summary>
        /// Edit product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var model = await Context.Products
                .Include(x => x.ProductPrices)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var result = model.Adapt<ProductViewModel>();

            result.Brands = new List<SelectListItem>();
            result.ProductAttributesList = new Dictionary<string, IEnumerable<SelectListItem>>();
            result.ProductCategoryList = new List<ProductCategoryDto>();
            result.ProductTypeList = new List<SelectListItem>();
            result.Price = result.PriceWithoutDiscount;
            result.ProductOption = new List<SelectListItem>();


            return View(GetDropdownItems(result));
        }

        /// <inheritdoc />
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid || model.Id.Equals(Guid.Empty))
            {
                ModelState.AddCommerceError(CommerceErrorKeys.InvalidModel);
                return View(model);
            }

            var dbModel = await Context.Products
                .Include(x => x.ProductPrices)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            dbModel.Name = model.Name;
            dbModel.DisplayName = model.DisplayName;
            dbModel.Description = model.Description;
            dbModel.IsPublished = model.IsPublished;
            dbModel.ShortDescription = model.ShortDescription;
            dbModel.Specification = model.Specification;
            dbModel.BrandId = model.BrandId;
            dbModel.ProductTypeId = model.ProductTypeId;

            if (model.ProductAttributesList != null && model.ProductImagesList.Any())
            {
                dbModel.ProductImages = model.ProductImagesList.Select(async x =>
                {
                    var stream = new MemoryStream();
                    await x.CopyToAsync(stream);
                    return stream;
                }).Select(x => x.Result).Select(x => x.ToArray()).Select(x => new ProductImage
                {
                    Image = x,
                    ProductId = model.Id
                }).ToList();
            }

            if (model.Price.Equals(dbModel.PriceWithoutDiscount).Negate())
            {
                Context.ProductPrices.Add(new ProductPrice
                {
                    Price = model.Price,
                    ProductId = model.Id
                });
            }

            Context.Products.Update(dbModel);
            var dbResult = await Context.PushAsync();

            if (dbResult.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AppendResultModelErrors(dbResult.Errors);

            return View(model);
        }

        /// <summary>
        /// Get product images
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetProductImages(Guid? productId)
        {
            var imagesRequest = await _galleryManager.GetProductImagesOnEditModeAsync(productId);
            return Json(!imagesRequest.IsSuccess ? new JsonFiles(null) : imagesRequest.Result);
        }

        /// <summary>
        /// Upload images
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadImages(UploadImagesViewModel model)
        {
            var result = await _galleryManager.UploadProductImagesAsync(model);
            return !result.IsSuccess ? Json("Error") : Json(result.Result);
        }

        /// <summary>
        /// Delete image by id
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(Guid? imageId)
        {
            await _galleryManager.DeleteImageAsync(imageId);
            return Json("OK");
        }

        /// <summary>
        /// Get user image
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult GetImage(Guid? imageId)
        {
            if (imageId == null) return NotFound();
            try
            {
                var photo = _productService.Context.ProductImages.SingleOrDefault(x => x.Id == imageId);
                if (photo?.Image != null) return File(photo.Image, photo.ContentType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }


        /// <summary>
        /// Get user image
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult GetImageThumb(Guid? imageId)
        {
            if (imageId == null) return NotFound();
            try
            {
                var photo = _productService.Context.ProductImages.SingleOrDefault(x => x.Id == imageId);
                if (photo?.Image != null)
                {
                    var resizedImage = photo.Image
                        .ToMemoryStream()
                        .GetImageFromStream()
                        .ResizeImage(80, 80)
                        .ToBytes(ImageFormat.Png);

                    return File(resizedImage, photo.ContentType);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }

        #endregion

        #region Api's

        /// <summary>
        /// Remove variation option
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="variationId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> RemoveOptione([Required]Guid? productId, [Required]Guid? variationId)
        {
            var resultModel = new ResultModel();
            if (productId is null || variationId is null)
            {
                resultModel.Errors.Add(new ErrorModel(string.Empty, "Invalid parameters"));
                return Json(resultModel);
            }

            var result = Context.ProductVariations
                .FirstOrDefault(x => x.Id == variationId && x.ProductId == productId);

            if (result == null) return Json(resultModel);
            Context.ProductVariations.Remove(result);
            var dbResult = await Context.PushAsync();
            return Json(dbResult);
        }

        /// <summary>
        /// Remove variation option
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> GetPriceByVariation(ProductPriceVariationViewModel model)
        {
            if (ModelState.IsValid) return await JsonAsync(_productService.GetPriceByVariationAsync(model));
            ModelState.AddCommerceError(CommerceErrorKeys.InvalidModel);
            return Json(model);
        }

        /// <summary>
        /// Get subscription plans
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]"), AllowAnonymous]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<SubscriptionPlanViewModel>>))]
        public async Task<JsonResult> GetSubscriptionPlans() =>
            await JsonAsync(_productService.GetSubscriptionPlansAsync());

        /// <summary>
        /// Get commerce statistic
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<SalesStatisticViewModel>))]
        public async Task<JsonResult> GetCommerceGeneralStatistic(DateTime? startDate, DateTime? endDate) =>
            await JsonAsync(_productService.GetCommerceGeneralStatisticAsync(startDate, endDate));

        /// <summary>
        /// Get year report
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Dictionary<int, object>>))]
        public async Task<JsonResult> GetYearReport() =>
            await JsonAsync(_productService.GetYearReportAsync());

        /// <summary>
        /// Get global currency
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/[controller]/[action]"), AllowAnonymous]
        [Produces("application/json", Type = typeof(ResultModel<Currency>))]
        public async Task<JsonResult> GetGlobalCurrency() =>
            Json(await _productService.GetGlobalCurrencyAsync());


        /// <summary>
        /// Set global currency
        /// </summary>
        /// <param name="currencyIdentifier"></param>
        /// <returns></returns>
        [Authorize(Roles = GlobalResources.Roles.ADMINISTRATOR)]
        [HttpPost, Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Currency>))]
        public async Task<JsonResult> SetGlobalCurrency(string currencyIdentifier) =>
            Json(await _productService.SetGlobalCurrencyAsync(currencyIdentifier));

        /// <summary>
        /// Load dropdown items
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ProductViewModel GetDropdownItems(ProductViewModel model)
        {
            model.Brands.AddRange(Context.Brands.Where(x => x.IsDeleted == false).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }));

            model.ProductAttributesList = Context.ProductAttribute.Include(x => x.AttributeGroup)
                .GroupBy(x => x.AttributeGroup.Name)
                .ToDictionary(grouping => grouping.Key, x => x.ToList().Select(w => new SelectListItem
                {
                    Text = w.Name,
                    Value = w.Id.ToString(),
                }));

            model.ProductOption = Context.ProductOption.ToList().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
            }).ToList();


            model.ProductCategoryList = Context.Categories.Select(x => new ProductCategoryDto
            {
                Name = x.Name,
                CategoryId = x.Id,
                IsChecked = Context.ProductCategories.Any(
                    w => w.ProductId.Equals(model.Id) && w.CategoryId.Equals(x.Id))
            }).ToList();

            model.ProductTypeList.AddRange(Context.ProductTypes.Where(x => x.IsDeleted == false).Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }));


            return model;
        }


        public List<SelectListItem> GetProdOptionByVariation(Guid productId)
        {
            return Context.ProductVariationDetails.Where(x => x.ProductVariation.ProductId == productId).Select(s => new SelectListItem
            {
                Text = s.ProductOption.Name,
                Value = s.ProductOptionId.ToString(),
            }).AsEnumerable().DistinctBy(d => d.Value).ToList();
        }

        public List<ProductVariation> GetProdVariationList(Guid productId)
        {

            var listVariation = Context.ProductVariations
                .Include(i => i.ProductVariationDetails)
                .ThenInclude(i => i.ProductOption)
                .Where(x => x.ProductId == productId).ToList();

            return listVariation;
        }


        [HttpPost]
        public async Task<JsonResult> EditProductAttributes([FromBody] IEnumerable<ProductAttributesViewModel> model)
        {
            foreach (var item in model)
            {
                var attribute = Context.ProductAttributes
                    .FirstOrDefault(x =>
                        x.ProductAttributeId == item.ProductAttributeId && x.ProductId == item.ProductId);

                if (attribute != null)
                {
                    Context.ProductAttributes.Remove(attribute);
                }

                Context.ProductAttributes.Add(item);
            }

            var dbResult = await Context.PushAsync();
            return Json(dbResult);
        }


        [HttpPost]
        public async Task<JsonResult> SaveProductVariation([FromBody] ProductVariationViewModel model)
        {
            var response = new ResultModel();

            if (!ModelState.IsValid)
            {
                response.Errors = ModelState.ToResultModelErrors().ToList();
                return Json(response);
            }

            var prod = await Context.Products.FirstOrDefaultAsync(i => i.Id == model.ProductId);

            if (prod != null)
            {
                var variation = await Context.ProductVariations
                    .FirstOrDefaultAsync(i => i.Id == model.VariationId && i.ProductId == model.ProductId);


                if (variation != null)
                {
                    variation.Price = model.Price;
                    Context.ProductVariations.Update(variation);

                    var listProductVariationDetails =
                        Context.ProductVariationDetails.Where(x => x.ProductVariationId == variation.Id);

                    Context.ProductVariationDetails.RemoveRange(listProductVariationDetails);
                }
                else
                {
                    variation = new ProductVariation
                    {
                        ProductId = model.ProductId,
                        Price = model.Price,
                    };

                    Context.ProductVariations.Add(variation);
                }

                var variationDetails = model.ProductVariationDetails.Select(x => new ProductVariationDetail()
                {
                    ProductVariationId = variation.Id,
                    Value = x.Value,
                    ProductOptionId = x.ProductOptionId
                });

                await Context.ProductVariationDetails.AddRangeAsync(variationDetails);
            }

            var dbResult = await Context.PushAsync();
            return Json(dbResult);
        }


        [HttpGet]
        public JsonResult GetProductVariation(string productId) => Json(Context.ProductVariations
            .Include(x => x.ProductVariationDetails).ThenInclude(x => x.ProductOption)
            .Where(x => x.ProductId == productId.ToGuid())
            .Select(x => new
            {
                VariationId = x.Id,
                x.Price,
                VariationDetails = x.ProductVariationDetails.Select(s => new { s.Value, Option = s.ProductOption.Name })
            }));


        [HttpGet]
        public JsonResult GetProductVariationById(string productId, string variationId) => Json(Context
            .ProductVariations
            .Include(x => x.ProductVariationDetails).ThenInclude(x => x.ProductOption)
            .Where(x => x.ProductId == productId.ToGuid() && x.Id == variationId.ToGuid())
            .Select(x => new
            {
                x.ProductId,
                VariationId = x.Id,
                x.Price,
                VariationDetails = x.ProductVariationDetails.Select(s => new { s.Value, Option = s.ProductOption.Name, optionId = s.ProductOptionId })
            }).FirstOrDefault());


        [HttpGet]
        public JsonResult GetProductAttributes(string productId) => Json(Context.ProductAttributes
            .Include(x => x.ProductAttribute)
            .Where(x => x.ProductId == productId.ToGuid())
            .Select(x => new
            {
                AttributeId = x.ProductAttributeId,
                Label = x.ProductAttribute.Name,
                x.Value,
                x.IsAvailable,
                x.IsPublished
            }));


        /// <summary>
        /// Remove attribute
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveAttribute(Guid productId, Guid attributeId)
            => await JsonAsync(_productService.RemoveAttributeAsync(productId, attributeId));

        /// <summary>
        /// Edit map product categories
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> EditProductCategories([FromBody] IEnumerable<ProductCategoriesViewModel> model)
        {
            foreach (var item in model)
            {
                if (Context.ProductCategories.Any(x =>
                    x.CategoryId == item.CategoryId && x.ProductId == item.ProductId))
                {
                    if (!item.Checked)
                    {
                        Context.ProductCategories.Remove(item);
                    }
                }
                else
                {
                    if (item.Checked)
                    {
                        Context.ProductCategories.Add(item);
                    }
                }
            }

            var dbResult = await Context.PushAsync();
            return Json(dbResult);
        }

        [HttpGet]
        public JsonResult GetProductCategories(Guid productId)
            => Json(Context.ProductCategories.Where(x => x.ProductId == productId));
        #endregion
    }
}