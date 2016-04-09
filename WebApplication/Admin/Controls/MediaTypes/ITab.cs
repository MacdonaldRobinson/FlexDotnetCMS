using FrameworkLibrary;
using System.Collections.Generic;

namespace WebApplication.Admin
{
    public interface ITab
    {
        void SetObject(IMediaDetail selectedItem);

        void UpdateFieldsFromObject();

        void UpdateObjectFromFields();
    }
}