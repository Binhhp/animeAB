﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeAB.Domain.ValueObjects;
using AnimeAB.Application.Reponsitories.Base;
using AnimeAB.Application.Extensions;

namespace AnimeAB.AppAdmin.Controllers
{
    [Authorize(Policy = RoleSchemes.Admin)]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Route("anime/dashboard")]
        public async Task<IActionResult> Index()
        {
            var animes = await unitOfWork.AnimeEntity.GetAnimesAsync();
            ViewData["Categories"] = (await unitOfWork.CategoriesEntity.GetCategoriesAsync()).Count();
            ViewData["Animes"] = animes.Count();
            ViewData["Collections"] = (await unitOfWork.CollectionEntity.GetCollectionsAsync()).Count();
            ViewData["Users"] = (await unitOfWork.AccountEntity.GetUsersAsync()).Count();
            return View();
        }

        [Route("anime/analysis")]
        public async Task<IActionResult> Analysis()
        {
            var animes = await unitOfWork.AnimeEntity.GetAnimesAsync();
            var monday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var mondayAnimeCreated = animes.Where(x => x.DateCreated.Date == monday.Date).Count();
            return Ok(mondayAnimeCreated);
        }

        [Route("Unauthorized")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("/NotFound")]
        public IActionResult NotFoundView()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
