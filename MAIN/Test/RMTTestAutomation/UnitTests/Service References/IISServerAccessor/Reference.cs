﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OPX.RenderingTest.IISServerAccessor {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="IISServerAccessor.IUpdateWebConfig")]
    public interface IUpdateWebConfig {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateWebConfig/UpdateConfigurationAppSettings", ReplyAction="http://tempuri.org/IUpdateWebConfig/UpdateConfigurationAppSettingsResponse")]
        bool UpdateConfigurationAppSettings(string key, string value, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateWebConfig/UpdateConfigurationAppSettings", ReplyAction="http://tempuri.org/IUpdateWebConfig/UpdateConfigurationAppSettingsResponse")]
        System.Threading.Tasks.Task<bool> UpdateConfigurationAppSettingsAsync(string key, string value, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateWebConfig/FileContains", ReplyAction="http://tempuri.org/IUpdateWebConfig/FileContainsResponse")]
        bool FileContains(string path, string key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateWebConfig/FileContains", ReplyAction="http://tempuri.org/IUpdateWebConfig/FileContainsResponse")]
        System.Threading.Tasks.Task<bool> FileContainsAsync(string path, string key);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUpdateWebConfigChannel : OPX.RenderingTest.IISServerAccessor.IUpdateWebConfig, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UpdateWebConfigClient : System.ServiceModel.ClientBase<OPX.RenderingTest.IISServerAccessor.IUpdateWebConfig>, OPX.RenderingTest.IISServerAccessor.IUpdateWebConfig {
        
        public UpdateWebConfigClient() {
        }
        
        public UpdateWebConfigClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UpdateWebConfigClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UpdateWebConfigClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UpdateWebConfigClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool UpdateConfigurationAppSettings(string key, string value, string path) {
            return base.Channel.UpdateConfigurationAppSettings(key, value, path);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateConfigurationAppSettingsAsync(string key, string value, string path) {
            return base.Channel.UpdateConfigurationAppSettingsAsync(key, value, path);
        }
        
        public bool FileContains(string path, string key) {
            return base.Channel.FileContains(path, key);
        }
        
        public System.Threading.Tasks.Task<bool> FileContainsAsync(string path, string key) {
            return base.Channel.FileContainsAsync(path, key);
        }
    }
}
