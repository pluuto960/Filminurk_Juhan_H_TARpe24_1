using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.FavouriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class FavouriteListsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFavouriteListsServices _favouriteListsServices;
        // favouriteList services add later
        private readonly FilesServices _filesServices;
        public FavouriteListsController(FilminurkTARpe24Context context, FilesServices filesServices, IFavouriteListsServices favouriteListsServices)
        {
            _context = context;
            _filesServices = filesServices;
            _favouriteListsServices = favouriteListsServices;
        }
        public IActionResult Index()
        {
            var resultingLists = _context.FavouriteLists
                .OrderByDescending(y => y.ListCreateAt)  // sorteeri nimekiri langevas jarjekorras kuupaeva jargi
                .Select(x => new FavouriteListsIndexViewModel
                {
                    FavouriteListID = x.FavouriteListID,
                    ListBelongsToUser = x.ListBelongsToUser,
                    IsMovieOrActor = x.IsMovieOrActor,
                    ListName = x.ListName,
                    ListDescription = x.ListDescription,

                    ListCreateAt = x.ListCreateAt,
                    
                    Image =
                    (List<FavouriteListIndexImageViewModel>)_context.FilesToDatabase.Where(ml => ml.ListID == x.FavouriteListID)
                    .Select(li => new FavouriteListIndexImageViewModel()
                    {
                        ListID = li.ListID,
                        ImageID = li.ImageID,
                        ImageData = li.ImageData,
                        ImageTitle = li.ImageTitle,
                        Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(li.ImageData))
                    })
                    
                    // Image = x.Image.Select(img => new ImageViewModel
                    // {
                    //     ImageID = img.ImageID,
                    //     ExistingFilePath = img.ExistingFilePath
                    // }).ToList()  
                }
                );
            return View(resultingLists);
        }



        /* create get, create post*/
        [HttpGet]
        public IActionResult Create()
        {
            //TODO identify the user type, return different views for admin and registered user
            
            var movies= _context.Movies
                .OrderBy(m=>m.Title)
                .Select(mo=>new MoviesIndexViewModel
                {
                    ID= mo.ID,
                    Title= mo.Title,
                    FirstPublished= mo.FirstPublished,
                    Genre= mo.Genre,
                })
                .ToList();
            ViewData["allmovies"]=movies;
            ViewData["userHasSelected"]= new List<string>();
            //this for normal user
            FavouriteListUserCreateViewModel vm = new();
            return View("UserCreate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> UserCreate(FavouriteListUserCreateViewModel vm, List<string> userHasSelected, List<MoviesIndexViewModel> movies)
        {
            List<Guid> tempParse = new();
            foreach (var stringID in userHasSelected)
            {
                tempParse.Add(Guid.Parse(stringID));
            }

            var newListDto = new FavouriteListDTO() { };
            newListDto.ListName = vm.ListName;
            newListDto.ListDescription = vm.ListDescription;
            newListDto.IsMovieOrActor = vm.IsMovieOrActor;
            newListDto.IsPrviate = vm.IsPrviate;
            newListDto.ListCreateAt=DateTime.UtcNow;
            newListDto.ListBelongsToUser = "00000000 - 0000 - 0000 - 000000000001";
            newListDto.ListDeletedAt= (DateTime)vm.ListDeletedAt;

            //List<Guid> convertedIDS=new List<Guid>();
            //if (newListDto.ListOfMovies != null)
            //{
            //    convertedIDS = MovieToId(newListDto.ListOfMovies);
            //}
            var newList = _favouriteListsServices.Create(newListDto);
            if (newList == null) 
            {
                return BadRequest(); 
            }
            return RedirectToAction("Index", vm);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(Guid id, Guid thisuserid)
        {
            if(id==null || thisuserid == null)
            {
                return BadRequest();
                //TODO: return corresponding errorviews. id not found for listm and user login error for userid
            }
            var thisList = _context.FavouriteLists.Where(tl => tl.FavouriteListID == id && tl.ListBelongsToUser == thisuserid.ToString()).Select(stl => new FavouriteListUserDetailsViewModel
            {
                FavouriteListID = stl.FavouriteListID,
                ListBelongsToUser = stl.ListBelongsToUser,
                IsMovieOrActor = stl.IsMovieOrActor,
                ListName = stl.ListName,
                ListDescription = stl.ListDescription,
                IsPrviate = stl.IsPrviate,
                ListOfMovies = stl.ListOfMovies,
                IsReported = stl.IsReported,
                Image = _context.FilesToDatabase
                .Where(i => i.ListID == stl.FavouriteListID)
                .Select(si => new FavouriteListIndexImageViewModel
                {
                    ImageID = si.ImageID,
                    ListID = si.ListID,
                    ImageData = si.ImageData,
                    ImageTitle = si.ImageTitle,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(si.ImageData))
                }).ToList().First()
            });
            //add viewdata attribute here later to discern between user and admin
            if(thisList == null)
            {
                return NotFound();
            }
            return View("Details",thisList);
        }

        private List<Guid> MovieToId(List<Movie> listOfMovies)
        {
            var result = new List<Guid>();
            foreach (var movie in listOfMovies)
            {
                result.Add(movie.ID);
            }
            return result;
        }//sss

    }
}
