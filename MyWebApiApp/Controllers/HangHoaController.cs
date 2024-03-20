using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Data;
using MyWebApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        private readonly MyDbContext _context;

        public HangHoaController(MyDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.HangHoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var hanghoa = _context.HangHoas.SingleOrDefault(x => x.MaHH == Guid.Parse(id));
                if (hanghoa == null)
                {
                    return NotFound();
                }
                return Ok(hanghoa);
            }
            catch
            {
                return BadRequest();
            }            
        }

        [HttpPost]
        public IActionResult Create(HangHoaVM hangHoaVM)
        {
            var hanghoa = new MyWebApiApp.Data.HangHoa
            {
                MaHH = Guid.NewGuid(),
                TenHH = hangHoaVM.TenHangHoa,
                DonGia = hangHoaVM.DonGia,
            };
            _context.HangHoas.Add(hanghoa);
            _context.SaveChanges();
            return Ok(new
            {
                Success = true,
                data = hanghoa
            });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(string id, MyWebApiApp.Data.HangHoa hangHoaEdit)
        {
            try
            {
                var hanghoa = _context.HangHoas.SingleOrDefault(x => x.MaHH == Guid.Parse(id));
                if (hanghoa == null)
                {
                    return NotFound();
                }

                if (id != hangHoaEdit.MaHH.ToString())
                {
                    return BadRequest();
                }
                //Update
                hanghoa.TenHH = hangHoaEdit.TenHH;
                hanghoa.DonGia = hangHoaEdit.DonGia;
                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(string id)
        {
            try
            {
                var hanghoa = _context.HangHoas.SingleOrDefault(x => x.MaHH == Guid.Parse(id));
                if (hanghoa == null)
                {
                    return NotFound();
                }
                //Update
                _context.HangHoas.Remove(hanghoa);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }   
}
