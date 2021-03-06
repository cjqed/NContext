﻿namespace NContext.ErrorHandling.Errors.Localization {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class NContextPersistenceError {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal NContextPersistenceError() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NContext.ErrorHandling.Errors.Localization.NContextPersistenceError", typeof(NContextPersistenceError).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UnitOfWorkBase instance Id: {0} with transaction Id: {1} has failed to commit due to the following exception(s): {2}.
        /// </summary>
        internal static string CommitFailed {
            get {
                return ResourceManager.GetString("CommitFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ScopeTransaction cannot be null. This is due to an invalid &quot;Func&lt;Transaction&gt; transactionFactory&quot; parameter being supplied to the UnitOfWorkBase constructor. ScopeTransaction is used for two things: 1. If IUnitOfWork.Commit() is called on the same thread with which the IUnitOfWork instance was created, then ScopeTransaction is the System.Transactions.CommittableTransaction that will be used; and 2. If IUnitOfWork.Commit() is called on a different thread from which the IUnitOfWork instance was created, then  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ScopeTransactionIsNull {
            get {
                return ResourceManager.GetString("ScopeTransactionIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UnitOfWorkBase instance id: {0} has already been committed..
        /// </summary>
        internal static string UnitOfWorkCommitted {
            get {
                return ResourceManager.GetString("UnitOfWorkCommitted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UnitOfWorkBase instance id: {0} is currently committing..
        /// </summary>
        internal static string UnitOfWorkCommitting {
            get {
                return ResourceManager.GetString("UnitOfWorkCommitting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UnitOfWorkBase instance cannot be committed..
        /// </summary>
        internal static string UnitOfWorkNonCommittable {
            get {
                return ResourceManager.GetString("UnitOfWorkNonCommittable", resourceCulture);
            }
        }
    }
}
