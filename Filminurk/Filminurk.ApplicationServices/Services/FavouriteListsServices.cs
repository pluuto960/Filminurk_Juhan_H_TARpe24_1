using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.ApplicationServices.Services
{
    public class FavouriteListsServices : IFavouriteListsServices
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFilesServices _filesServices;

        public FavouriteListsServices(FilminurkTARpe24Context context, IFilesServices filesServices)
        {
            _context = context;
            _filesServices = filesServices;
        }

        public async Task<FavouriteList> DetailsAsync(Guid id)
        {
            var result = await _context.FavouriteLists
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FavouriteListID == id);
            return result;

        }

        public async Task<FavouriteList> Create(FavouriteListDTO dto/*, List<Movie>selectedMovies*/)
        {
            FavouriteList newList = new();
            newList.FavouriteListID= Guid.NewGuid();
            newList.ListName=dto.ListName;
            newList.ListDescription=dto.ListDescription;
            newList.ListCreateAt= dto.ListCreateAt;
            newList.ListModifiedAt= dto.ListModifiedAt;
            newList.ListDeletedAt= dto.ListDeletedAt;
            newList.ListOfMovies= dto.ListOfMovies;
            await _context.FavouriteLists.AddAsync(newList);
            await _context.SaveChangesAsync();

            //foreach(var movieid in selectedMovies)
            //{
            //    _context.FavouriteLists.Entry
            //}
            return newList;
        }
        public async Task<FavouriteList> Update(FavouriteListDTO updatedList, string typeOfMethod)
        {
           
            FavouriteList updatedListInDB = new();

            updatedListInDB.FavouriteListID = updatedList.FavouriteListID;
            updatedListInDB.ListBelongsToUser= updatedList.ListBelongsToUser;
            updatedListInDB.IsMovieOrActor= updatedList.IsMovieOrActor;
            updatedListInDB.ListName= updatedList.ListName;
            updatedListInDB.ListDescription= updatedList.ListDescription;
            updatedListInDB.IsPrviate= updatedList.IsPrviate;
            updatedListInDB.ListOfMovies= updatedList.ListOfMovies;
            updatedListInDB.ListCreateAt = updatedList.ListCreateAt;
            updatedListInDB.ListDeletedAt = updatedList.ListDeletedAt;
            updatedListInDB.ListModifiedAt = updatedList.ListModifiedAt;
            if (typeOfMethod == "Delete")
            {
                _context.FavouriteLists.Attach(updatedListInDB);
                _context.Entry(updatedListInDB).Property(l => l.ListDeletedAt).IsModified = true;
                
            }
            else if (typeOfMethod == "Private")
            {
                _context.FavouriteLists.Attach(updatedListInDB);
                _context.Entry(updatedListInDB).Property(l => l.IsPrviate).IsModified = true;
            }
            _context.Entry(updatedListInDB).Property(l => l.ListModifiedAt).IsModified = true;
            await _context.SaveChangesAsync();
            return updatedListInDB;


        }
    }
}
