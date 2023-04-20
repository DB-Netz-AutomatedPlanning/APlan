﻿#pragma checksum "..\..\..\..\Views\Draw.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "90F6CCCECE6FEB94AB7E9D84C38B7DC48F261F4F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using APLan;
using APLan.Commands;
using APLan.Converters;
using APLan.HelperClasses;
using APLan.Model.CustomObjects;
using APLan.ViewModels;
using APLan.Views;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using Microsoft.Xaml.Behaviors.Input;
using Microsoft.Xaml.Behaviors.Layout;
using Microsoft.Xaml.Behaviors.Media;
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


namespace APLan.Views {
    
    
    /// <summary>
    /// Draw
    /// </summary>
    public partial class Draw : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\..\Views\Draw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid CanvasGrid;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Views\Draw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition SignalOutput;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\Views\Draw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer DrawingViewer;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\Views\Draw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas mycanvas;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/APLan;component/views/draw.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Draw.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.CanvasGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.SignalOutput = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 3:
            this.DrawingViewer = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 4:
            this.mycanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 79 "..\..\..\..\Views\Draw.xaml"
            this.mycanvas.DragOver += new System.Windows.DragEventHandler(this.drawingCanvas_DragOver);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

