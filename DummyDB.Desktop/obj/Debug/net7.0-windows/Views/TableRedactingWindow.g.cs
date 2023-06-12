﻿#pragma checksum "..\..\..\..\Views\TableRedactingWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BCA1D40EC6CBF4497DB4B418758677F70E2ACB65"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DummyDB.Desktop.Views {
    
    
    /// <summary>
    /// TableRedactingWindow
    /// </summary>
    public partial class TableRedactingWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\..\Views\TableRedactingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border SchemaRedactingPanel;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Views\TableRedactingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TableName;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Views\TableRedactingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Columns;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\Views\TableRedactingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border DataRedactingPanel;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Views\TableRedactingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Rows;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\Views\TableRedactingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border TableFrame;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\Views\TableRedactingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid TableGrid;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\..\Views\TableRedactingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox IndexForDelete;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DummyDB.Desktop;component/views/tableredactingwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\TableRedactingWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.SchemaRedactingPanel = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.TableName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.Columns = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.DataRedactingPanel = ((System.Windows.Controls.Border)(target));
            return;
            case 5:
            this.Rows = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this.TableFrame = ((System.Windows.Controls.Border)(target));
            return;
            case 7:
            this.TableGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.IndexForDelete = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

