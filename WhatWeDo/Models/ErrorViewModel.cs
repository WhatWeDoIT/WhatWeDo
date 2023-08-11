namespace WhatWeDo.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        //hola caracola
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}