using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Models.OMDb;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class OMDbController : Controller
    {
        private readonly IOMDbServices _omDbServices;
        private readonly IMovieServices _movieServices;

        public OMDbController(IOMDbServices omDbServices, IMovieServices movieServices)
        {
            _omDbServices = omDbServices;
            _movieServices = movieServices;
        }

        // GET: näitab vormi
        public IActionResult Index()
        {
            return View(new OMDbImportViewModel());
        }

        // POST: otsib OMDb-st ja salvestab andmebaasi
        [HttpPost]
        public async Task<IActionResult> Import(OMDbImportViewModel vm)
        {
            var omdb = await _omDbServices.GetMovieByTitle(vm.SearchTitle);

            if (omdb.Response == "False")
            {
                vm.Message = $"Film ei leitud: {omdb.Error}";
                return View("Index", vm);
            }

            // Konverteeri OMDbDTO → MoviesDTO
            var dto = new MoviesDTO
            {
                Title = omdb.Title,
                Description = omdb.Plot,
                Director = omdb.Director,
                CurrentRating = double.TryParse(omdb.imdbRating, out var r) ? r : null,
                Actors = omdb.Actors?.Split(", ").ToList(),
                FirstPublished = DateOnly.TryParseExact(omdb.Released,"dd MMM yyyy",System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None,out var d) ? d : DateOnly.MinValue,
                Files = new List<IFormFile>()  // tühi, sest pilti importida ei saa
            };

            await _movieServices.Create(dto);

            vm.Message = $"Film '{omdb.Title}' edukalt lisatud!";
            return View("Index", vm);
        }
    }


}
