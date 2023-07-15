using System;
using System.Collections.Generic;
using PagedList;
using PagedList.Mvc;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MaleFashion.Models;

namespace MaleFashion.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        QLTHOITRANGContext dB = new QLTHOITRANGContext();
        // GET: Shop

        public ActionResult Index(int? page, string sortBy, string searchString, string category, int? minPrice, int? maxPrice)
        {
            // Default sort by price, low to high.
            ViewBag.SortBy = string.IsNullOrEmpty(sortBy) ? "price_asc" : sortBy;
            ViewBag.SearchString = searchString;
            ViewBag.Category = category;
            ViewBag.Min = minPrice;
            ViewBag.Max = maxPrice;
            var pageSize = 9;
            var pageNumber = page ?? 1;
            var products= dB.SanPhams.AsQueryable();

            // Search by book name.
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.TenSP.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
               products  = products.Where(p => p.TheLoai.TenTL == category);
            }
      


            if (minPrice.HasValue && maxPrice.HasValue)
            {
                products = products.Where(p => p.GiaBan >= minPrice.Value && p.GiaBan <= maxPrice.Value);
            }
            else if (minPrice.HasValue)
            {
                products = products.Where(p => p.GiaBan >= minPrice.Value);
            }
            else if (maxPrice.HasValue)
            {
                products = products.Where(p => p.GiaBan <= maxPrice.Value);
            }

            // Sort by price.
            switch (sortBy)
            {
                case "price_desc":
                    products = products.OrderByDescending(p => p.GiaBan);
                    break;
                case "price_asc":
                default:
                    products = products.OrderBy(p => p.GiaBan);
                    break;
            }
            var totalCount = products.Count();
            var pagedList = products.ToPagedList(pageNumber, pageSize);
            var categories = dB.TheLoais.ToList();
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Categories = categories;

            return View(pagedList);
        }




        public ActionResult ShopDetails(string id)
        {
            SanPham products = dB.SanPhams.FirstOrDefault(x => x.MaSP == id);
            var theme = (from SanPham in dB.SanPhams
                         join TheLoai in dB.TheLoais on SanPham.MaTL equals TheLoai.MaTL
                         where SanPham.MaSP == id
                         select TheLoai.TenTL).FirstOrDefault();


            var relatedProducts = dB.SanPhams.Where(x => x.MaTL == products.MaTL && x.MaSP != products.MaSP)
                               .OrderBy(x => Guid.NewGuid())
                               .Take(4) 
                               .Distinct() 
                               .ToList();

            ViewBag.ChuDe = theme;
            ViewBag.RelatedProducts = relatedProducts;
            return View(products);
        }

    }
}