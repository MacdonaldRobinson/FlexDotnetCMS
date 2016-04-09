using FrameworkLibrary;
using System.Collections.Generic;

namespace WebApplication.Admin
{
    public interface IMediaDetailPanel
    {
        void UpdateObjectFromFields();

        void UpdateFieldsFromObject();

        void SetObject(IMediaDetail obj);

        IMediaDetail SelectedItem { get; }

        List<Tab> Tabs { get; }
    }
}