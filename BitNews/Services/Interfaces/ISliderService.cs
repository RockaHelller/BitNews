using BitNews.Areas.Admin.ViewModels;
using BitNews.Areas.Admin.ViewModels.Slider;
using BitNews.Models;

namespace BitNews.Services.Interfaces
{
	public interface ISliderService
	{
        Task<List<Slider>> GetAllAsync();
        Task<Slider> GetByIdAsync(int id);
        Task<List<SliderVM>> GetAllMappedDatasAsync();
        SliderDetailVM GetMappedData(Slider slider);
        Task CreateAsync(List<IFormFile> images, string title, string description);
        Task DeleteAsync(int id);
        Task DeleteAllAsync(int id);

        Task EditAsync(int id, SliderEditVM model);
        Task<List<Slider>> GetAllByStatusAsync();
        Task<int> GetCountAsync();
        Task<bool> ChangeStatusAsync(Slider slider);
        Task<Slider> GetWithIncludesAsync(int id);
    }
}
