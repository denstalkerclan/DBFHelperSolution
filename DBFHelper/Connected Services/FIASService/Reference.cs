﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBFHelper.FIASService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DownloadFileInfo", Namespace="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/")]
    [System.SerializableAttribute()]
    public partial class DownloadFileInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long VersionIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TextVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FiasCompleteDbfUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FiasCompleteXmlUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FiasDeltaDbfUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FiasDeltaXmlUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Kladr4ArjUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Kladr47ZUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GarXMLFullURLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GarXMLDeltaURLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public long VersionId {
            get {
                return this.VersionIdField;
            }
            set {
                if ((this.VersionIdField.Equals(value) != true)) {
                    this.VersionIdField = value;
                    this.RaisePropertyChanged("VersionId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string TextVersion {
            get {
                return this.TextVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.TextVersionField, value) != true)) {
                    this.TextVersionField = value;
                    this.RaisePropertyChanged("TextVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string FiasCompleteDbfUrl {
            get {
                return this.FiasCompleteDbfUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.FiasCompleteDbfUrlField, value) != true)) {
                    this.FiasCompleteDbfUrlField = value;
                    this.RaisePropertyChanged("FiasCompleteDbfUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string FiasCompleteXmlUrl {
            get {
                return this.FiasCompleteXmlUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.FiasCompleteXmlUrlField, value) != true)) {
                    this.FiasCompleteXmlUrlField = value;
                    this.RaisePropertyChanged("FiasCompleteXmlUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string FiasDeltaDbfUrl {
            get {
                return this.FiasDeltaDbfUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.FiasDeltaDbfUrlField, value) != true)) {
                    this.FiasDeltaDbfUrlField = value;
                    this.RaisePropertyChanged("FiasDeltaDbfUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string FiasDeltaXmlUrl {
            get {
                return this.FiasDeltaXmlUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.FiasDeltaXmlUrlField, value) != true)) {
                    this.FiasDeltaXmlUrlField = value;
                    this.RaisePropertyChanged("FiasDeltaXmlUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string Kladr4ArjUrl {
            get {
                return this.Kladr4ArjUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.Kladr4ArjUrlField, value) != true)) {
                    this.Kladr4ArjUrlField = value;
                    this.RaisePropertyChanged("Kladr4ArjUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string Kladr47ZUrl {
            get {
                return this.Kladr47ZUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.Kladr47ZUrlField, value) != true)) {
                    this.Kladr47ZUrlField = value;
                    this.RaisePropertyChanged("Kladr47ZUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string GarXMLFullURL {
            get {
                return this.GarXMLFullURLField;
            }
            set {
                if ((object.ReferenceEquals(this.GarXMLFullURLField, value) != true)) {
                    this.GarXMLFullURLField = value;
                    this.RaisePropertyChanged("GarXMLFullURL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string GarXMLDeltaURL {
            get {
                return this.GarXMLDeltaURLField;
            }
            set {
                if ((object.ReferenceEquals(this.GarXMLDeltaURLField, value) != true)) {
                    this.GarXMLDeltaURLField = value;
                    this.RaisePropertyChanged("GarXMLDeltaURL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string Date {
            get {
                return this.DateField;
            }
            set {
                if ((object.ReferenceEquals(this.DateField, value) != true)) {
                    this.DateField = value;
                    this.RaisePropertyChanged("Date");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/", ConfigurationName="FIASService.IDownloadService")]
    public interface IDownloadService {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/GetAllDownloadFileI" +
            "nfo", ReplyAction="*")]
        System.Collections.Generic.List<DBFHelper.FIASService.DownloadFileInfo> GetAllDownloadFileInfo();
        
        [System.ServiceModel.OperationContractAttribute(Action="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/GetAllDownloadFileI" +
            "nfo", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<DBFHelper.FIASService.DownloadFileInfo>> GetAllDownloadFileInfoAsync();
        
        // CODEGEN: Контракт генерации сообщений с именем GetLastDownloadFileInfoResult из пространства имен https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/ не отмечен как обнуляемый
        [System.ServiceModel.OperationContractAttribute(Action="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/GetLastDownloadFile" +
            "Info", ReplyAction="*")]
        DBFHelper.FIASService.GetLastDownloadFileInfoResponse GetLastDownloadFileInfo(DBFHelper.FIASService.GetLastDownloadFileInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/GetLastDownloadFile" +
            "Info", ReplyAction="*")]
        System.Threading.Tasks.Task<DBFHelper.FIASService.GetLastDownloadFileInfoResponse> GetLastDownloadFileInfoAsync(DBFHelper.FIASService.GetLastDownloadFileInfoRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLastDownloadFileInfoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLastDownloadFileInfo", Namespace="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/", Order=0)]
        public DBFHelper.FIASService.GetLastDownloadFileInfoRequestBody Body;
        
        public GetLastDownloadFileInfoRequest() {
        }
        
        public GetLastDownloadFileInfoRequest(DBFHelper.FIASService.GetLastDownloadFileInfoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class GetLastDownloadFileInfoRequestBody {
        
        public GetLastDownloadFileInfoRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLastDownloadFileInfoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLastDownloadFileInfoResponse", Namespace="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/", Order=0)]
        public DBFHelper.FIASService.GetLastDownloadFileInfoResponseBody Body;
        
        public GetLastDownloadFileInfoResponse() {
        }
        
        public GetLastDownloadFileInfoResponse(DBFHelper.FIASService.GetLastDownloadFileInfoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://fias.nalog.ru/WebServices/Public/DownloadService.asmx/")]
    public partial class GetLastDownloadFileInfoResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public DBFHelper.FIASService.DownloadFileInfo GetLastDownloadFileInfoResult;
        
        public GetLastDownloadFileInfoResponseBody() {
        }
        
        public GetLastDownloadFileInfoResponseBody(DBFHelper.FIASService.DownloadFileInfo GetLastDownloadFileInfoResult) {
            this.GetLastDownloadFileInfoResult = GetLastDownloadFileInfoResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDownloadServiceChannel : DBFHelper.FIASService.IDownloadService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DownloadServiceClient : System.ServiceModel.ClientBase<DBFHelper.FIASService.IDownloadService>, DBFHelper.FIASService.IDownloadService {
        
        public DownloadServiceClient() {
        }
        
        public DownloadServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DownloadServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DownloadServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DownloadServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<DBFHelper.FIASService.DownloadFileInfo> GetAllDownloadFileInfo() {
            return base.Channel.GetAllDownloadFileInfo();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<DBFHelper.FIASService.DownloadFileInfo>> GetAllDownloadFileInfoAsync() {
            return base.Channel.GetAllDownloadFileInfoAsync();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DBFHelper.FIASService.GetLastDownloadFileInfoResponse DBFHelper.FIASService.IDownloadService.GetLastDownloadFileInfo(DBFHelper.FIASService.GetLastDownloadFileInfoRequest request) {
            return base.Channel.GetLastDownloadFileInfo(request);
        }
        
        public DBFHelper.FIASService.DownloadFileInfo GetLastDownloadFileInfo() {
            DBFHelper.FIASService.GetLastDownloadFileInfoRequest inValue = new DBFHelper.FIASService.GetLastDownloadFileInfoRequest();
            inValue.Body = new DBFHelper.FIASService.GetLastDownloadFileInfoRequestBody();
            DBFHelper.FIASService.GetLastDownloadFileInfoResponse retVal = ((DBFHelper.FIASService.IDownloadService)(this)).GetLastDownloadFileInfo(inValue);
            return retVal.Body.GetLastDownloadFileInfoResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<DBFHelper.FIASService.GetLastDownloadFileInfoResponse> DBFHelper.FIASService.IDownloadService.GetLastDownloadFileInfoAsync(DBFHelper.FIASService.GetLastDownloadFileInfoRequest request) {
            return base.Channel.GetLastDownloadFileInfoAsync(request);
        }
        
        public System.Threading.Tasks.Task<DBFHelper.FIASService.GetLastDownloadFileInfoResponse> GetLastDownloadFileInfoAsync() {
            DBFHelper.FIASService.GetLastDownloadFileInfoRequest inValue = new DBFHelper.FIASService.GetLastDownloadFileInfoRequest();
            inValue.Body = new DBFHelper.FIASService.GetLastDownloadFileInfoRequestBody();
            return ((DBFHelper.FIASService.IDownloadService)(this)).GetLastDownloadFileInfoAsync(inValue);
        }
    }
}