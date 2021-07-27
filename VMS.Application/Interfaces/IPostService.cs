using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IPostService
    {
        Task<List<PostViewModel>> PostsWithCategory();

        Task AddPostWithCat(CreatePostWithCatViewModel postWithCatViewModel);

        Task<CreatePostWithCatViewModel> GetPostWithCat(int id);

        Task UpdatePostWithCat(int id, CreatePostWithCatViewModel postWithCatViewModel);

        Task DeletePost(int id);
    }
}