﻿using BitNews.Data;
using BitNews.Models;
using BitNews.Services.Interfaces;
using BitNews.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BitNews.Services
{
	public class LayoutService : ILayoutService
	{
		private readonly AppDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly INewsService _newsService;


		public LayoutService(AppDbContext context,
							 IHttpContextAccessor httpContextAccessor,
							 UserManager<AppUser> userManager,
							 INewsService newsService)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
			_newsService = newsService;

		}

        public async Task<LayoutVM> GetAllDatas()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userId != null ? await _userManager.FindByIdAsync(userId) : null;

            var datas = _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            var news = await _newsService.GetAllWithIncludesAsync();

            var layoutVM = new LayoutVM
            {
                SettingDatas = datas,
                UserFullName = user?.FullName ?? "Guest User", // Populate with "Guest User" when not logged in
                UserEmail = user?.Email,
                News = news.ToList(),
            };
                    
            return layoutVM;
        }


    }
}
