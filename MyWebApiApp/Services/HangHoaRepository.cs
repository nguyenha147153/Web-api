﻿using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApiApp.Services
{
    public class HangHoaRepository : IHangHoaRepository
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 2;
        public HangHoaRepository(MyDbContext context) 
        {
            _context = context;
        }
        public List<HangHoaModel> GetAll(string search,double? from, double? to, string sortBy, int page)
        {
            var allProducts = _context.HangHoas.Include(hh=>hh.Loai).AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                allProducts = allProducts.Where(hh => hh.TenHH.Contains(search));
            }

            if(from.HasValue)
            {
                allProducts = allProducts.Where(hh => hh.DonGia >= from);
            }
            if (to.HasValue)
            {
                allProducts = allProducts.Where(hh => hh.DonGia <= to);
            }
            #endregion
            #region Sorting
            //Default sort by name (tenhh)
            allProducts = allProducts.OrderBy(hh => hh.TenHH);

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch(sortBy)
                {
                    case "tenhh_desc": allProducts = allProducts.OrderByDescending(hh => hh.TenHH);break;
                    case "gia_asc": allProducts = allProducts.OrderBy(hh=>hh.DonGia);break;
                    case "gia_desc": allProducts = allProducts.OrderByDescending(hh => hh.DonGia); break;
                }
            }
            #endregion

            //#region Paging
            //allProducts = allProducts.Skip((page-1)*PAGE_SIZE).Take(PAGE_SIZE);
            //#endregion

            //var result = allProducts.Select(hh => new HangHoaModel
            //{
            //    MaHangHoa = hh.MaHH,
            //    TenHangHoa = hh.TenHH,
            //    DonGia = hh.DonGia,
            //    TenLoai = hh.Loai.TenLoai,
            //});
            //return result.ToList();

            var result = PaginatedList<MyWebApiApp.Data.HangHoa >.Create(allProducts, page, PAGE_SIZE);
            var number = result.TotalPage;

            return result.Select(hh => new HangHoaModel
            {
                MaHangHoa = hh.MaHH,
                TenHangHoa = hh.TenHH,
                DonGia = hh.DonGia,
                TenLoai = hh.Loai?.TenLoai,
            }).ToList();
        }
    }
}
