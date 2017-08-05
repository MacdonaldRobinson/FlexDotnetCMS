using System;

namespace FrameworkLibrary
{
    public interface IField : IMustContainID
    {
        long ID { get; set; }
        string FieldCode { get; set; }
        string FieldLabel { get; set; }
        string FieldValue { get; set; }
        long OrderIndex { get; set; }
        string FrontEndLayout { get; set; }
        System.DateTime DateCreated { get; set; }
        System.DateTime DateLastModified { get; set; }
        string GroupName { get; set; }
        bool RenderLabelAfterControl { get; set; }
        string AdminControl { get; set; }
        string GetAdminControlValue { get; set; }
        string SetAdminControlValue { get; set; }
        string FieldDescription { get; set; }
        bool ShowFrontEndFieldEditor { get; set; }
        string FrontEndSubmissions { get; set; }
    }
}