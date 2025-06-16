using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Repositories;

namespace trashtracker_api.Controllers
{
    public class FavoriteLocationsController : ControllerBase
    {
        private IFavoriteLocationsRepository _favoriteLocationsRepository;

        public FavoriteLocationsController (IFavoriteLocationsRepository favoriteLocationsRepository)
        {
            _favoriteLocationsRepository = favoriteLocationsRepository;
        }
    }
}
