#pragma warning disable 1591
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace WareHouseManagement.PCL.PrintTemplates
{
using System;

#line 2 "ListPrintTemplate.cshtml"
using System.Collections.Generic;

#line default
#line hidden
using System.Linq;
using System.Text;

#line 1 "ListPrintTemplate.cshtml"
using WareHouseManagement.PCL.Model;

#line default
#line hidden


[System.CodeDom.Compiler.GeneratedCodeAttribute("RazorTemplatePreprocessor", "2.6.0.0")]
public partial class ListPrintTemplate : ListPrintTemplateBase
{

#line hidden

#line 4 "ListPrintTemplate.cshtml"
public List<EZPrintModel> Model { get; set; }

#line default
#line hidden


public override void Execute()
{
WriteLiteral("\n");

WriteLiteral("<!DOCTYPE html>\n<html");

WriteLiteral(" lang=\"en\"");

WriteLiteral(">\n\t<head>\n\t\t<!-- iOS - needs to reside in the \"Resources\" directory and have buil" +
"d action of \"BundleResource\" -->\n\t\t<!-- Droid - needs to reside in \"Assets\" dire" +
"ctory and have build action of \"AndroidAsset\" -->\n\t\t<link");


WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" href=\"primer.css\"");
            WriteLiteral(">\n\t<link");
            WriteLiteral(" href=\"bootstrap.css\"");
            WriteLiteral("rel=\"stylesheet\"");

            WriteLiteral(">\n\t\t<script type =\"text/javascript\"");
            WriteLiteral("src=\"bootstrap.js\"");
                WriteLiteral(">\n\t\t</script>\n\t</head>\n\t<body>\n\t\t<div");

            WriteLiteral(" class=\"blankslate blankslate-spacious\"");

            WriteLiteral(">\n\t\t\t<h3>Dunblare Import-Export INC</h3>\n\t\t\t<p>11305 N.W. 122nd St.<br/>" +
                      " MEDLEY, FL 33178<br/>" +
                  "(308)883 - 1194<br/>" +
                " WWW.DUNBLARE.COM</p>\n\t\t</div><div");
            WriteLiteral("class=\"container\"");
            WriteLiteral(">\n\t\t<div");

            WriteLiteral(" class=\"top-bottom-border\"");
            WriteLiteral(">\n\t<h4");
            WriteLiteral(" class=\"align-top-left\"");
            WriteLiteral(">\n\tDIVISION:</h4>\n\t\t<div");
            WriteLiteral(" class=\"text-center\"");

            WriteLiteral(">\n\t<label");
            WriteLiteral(" class=\"textsize\"");
           WriteLiteral(">\n\t\tMassy US");
          WriteLiteral("</label>\n\t\t</div>\n\t\t</div>\n\t\t<div");
             WriteLiteral(" class=\"bottom-border\"");
            WriteLiteral(">\n\t\t<h4");
            WriteLiteral(" class=\"align-top-left\"");
            WriteLiteral(">\n\tCustomer:</h4>\n\t<div");
         
            WriteLiteral(" class=\"text-center\"");
            WriteLiteral(">\t\t\t<label class=\"textsize\">");
            WriteLiteral("Neal And Massy");
            WriteLiteral("\n\t</label>");
            WriteLiteral("\n\t\t</div>\n\t\t</div>\n\t\t<div");
            WriteLiteral(" class=\"one columns\"");
            WriteLiteral(">\n\t\t<div");
                WriteLiteral(" class=\"one-third column\"");

            WriteLiteral(">\n\t\t<h4>Receiving_Log#:</h4>\n\t</div>\n\t<div");
            WriteLiteral(" class=\"one-third column\"");
            WriteLiteral(">\n\t\t\t<h4>User_id:SJOSEPH</h4>\n\t\t</div>\n\t\t<div");
            WriteLiteral(" class=\"one-third column\"");
            WriteLiteral(">\n\t\t<h4>DateIn:01/02/2019</h4>\n\t\t</div>\n\t</div>\n\t\t<div");
            WriteLiteral(" class=\"text-center bottom-border\"");
            WriteLiteral(">\n\t<label class=\"textsize\">");
            WriteLiteral("\n\t001359");
            WriteLiteral("\n\t</label>\n\t\t</div>\n\t<div");
             WriteLiteral(" class=\"columns bottom-border\"");
            WriteLiteral(">\n\t\t<div");
            WriteLiteral(" class= \"col-md-12\"");
            WriteLiteral(">\n\t<div");
            WriteLiteral(" class=\"col-md-6\"");
            WriteLiteral(">\n\t<h4>Boxes:</h4>");
            WriteLiteral("\n\t<label class=\"label-font-size\">");
            WriteLiteral("\n\t2,140");
            WriteLiteral("\n\t\t</label>\n\t\t</div>\n\t\t<div");
            WriteLiteral(" class=\"col-md-6 left-border\"");
            
            
            WriteLiteral(">\n\t<h4>Pieces:</h4>");
            WriteLiteral("\n\t<label");
            WriteLiteral("class=\"label-font-size\">");
           
           WriteLiteral("\n\t2,140");
            WriteLiteral("\n\t\t</label>\n\t\t</div>\n\t\t</div>\n\t\t</div>\n\t<div");
           
            WriteLiteral(" class=\"bottom-border\"");

            WriteLiteral(">\n\t\t<h4 class=\"align-top-left\">");
            WriteLiteral("\n\tSKU/ITEM:</h4>");
            WriteLiteral("\n\t\t<div");
            WriteLiteral(" class=\"text-center\"");
            WriteLiteral(">\n\t<label class=\"textsize\">");
            WriteLiteral("Massy US");
            WriteLiteral("\n\t\t</label>\n\t\t\t</div>");
           
            WriteLiteral("\n\t\t</div>\n\t\t<div>");
         
            WriteLiteral("\n\t<h4 class=\"align-top-left\"");
            WriteLiteral(">\n\t\tBARCODE:</h4>");
            WriteLiteral("\n\t<div");
            WriteLiteral(" class=\"text-center\"");
            WriteLiteral(">\n\t<label class=\"textsize\">");
            WriteLiteral("Massy US");
            WriteLiteral("</label>\n\t\t</div>\n\t\t</div>\n\t\t</div>\n\t\t</body>\n</html>");
           

        }
    }

    // NOTE: this is the default generated helper class. You may choose to extract it to a separate file 
    // in order to customize it or share it between multiple templates, and specify the template's base 
    // class via the @inherits directive.
    public abstract class ListPrintTemplateBase
{

		// This field is OPTIONAL, but used by the default implementation of Generate, Write, WriteAttribute and WriteLiteral
		//
		System.IO.TextWriter __razor_writer;

		// This method is OPTIONAL
		//
		/// <summary>Executes the template and returns the output as a string.</summary>
		/// <returns>The template output.</returns>
		public string GenerateString ()
		{
			using (var sw = new System.IO.StringWriter ()) {
				Generate (sw);
				return sw.ToString ();
			}
		}

		// This method is OPTIONAL, you may choose to implement Write and WriteLiteral without use of __razor_writer
		// and provide another means of invoking Execute.
		//
		/// <summary>Executes the template, writing to the provided text writer.</summary>
		/// <param name="writer">The TextWriter to which to write the template output.</param>
		public void Generate (System.IO.TextWriter writer)
		{
			__razor_writer = writer;
			Execute ();
			__razor_writer = null;
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the template output without HTML escaping it.</summary>
		/// <param name="value">The literal value.</param>
		protected void WriteLiteral (string value)
		{
			__razor_writer.Write (value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the TextWriter without HTML escaping it.</summary>
		/// <param name="writer">The TextWriter to which to write the literal.</param>
		/// <param name="value">The literal value.</param>
		protected static void WriteLiteralTo (System.IO.TextWriter writer, string value)
		{
			writer.Write (value);
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a value to the template output, HTML escaping it if necessary.</summary>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected void Write (object value)
		{
			WriteTo (__razor_writer, value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes an object value to the TextWriter, HTML escaping it if necessary.</summary>
		/// <param name="writer">The TextWriter to which to write the value.</param>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected static void WriteTo (System.IO.TextWriter writer, object value)
		{
			if (value == null)
				return;

			var write = value as Action<System.IO.TextWriter>;
			if (write != null) {
				write (writer);
				return;
			}

			//NOTE: a more sophisticated implementation would write safe and pre-escaped values directly to the
			//instead of double-escaping. See System.Web.IHtmlString in ASP.NET 4.0 for an example of this.
			writer.Write(System.Net.WebUtility.HtmlEncode (value.ToString ()));
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to the template output.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		protected void WriteAttribute (string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			WriteAttributeTo (__razor_writer, name, prefix, suffix, values);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to a TextWriter.
		/// </summary>
		/// <param name="writer">The TextWriter to which to write the attribute.</param>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		///<remarks>Used by Razor helpers to write attributes.</remarks>
		protected static void WriteAttributeTo (System.IO.TextWriter writer, string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			// this is based on System.Web.WebPages.WebPageExecutingBase
			// Copyright (c) Microsoft Open Technologies, Inc.
			// Licensed under the Apache License, Version 2.0
			if (values.Length == 0) {
				// Explicitly empty attribute, so write the prefix and suffix
				writer.Write (prefix);
				writer.Write (suffix);
				return;
			}

			bool first = true;
			bool wroteSomething = false;

			for (int i = 0; i < values.Length; i++) {
				Tuple<string,object,bool> attrVal = values [i];
				string attPrefix = attrVal.Item1;
				object value = attrVal.Item2;
				bool isLiteral = attrVal.Item3;

				if (value == null) {
					// Nothing to write
					continue;
				}

				// The special cases here are that the value we're writing might already be a string, or that the 
				// value might be a bool. If the value is the bool 'true' we want to write the attribute name instead
				// of the string 'true'. If the value is the bool 'false' we don't want to write anything.
				//
				// Otherwise the value is another object (perhaps an IHtmlString), and we'll ask it to format itself.
				string stringValue;
				bool? boolValue = value as bool?;
				if (boolValue == true) {
					stringValue = name;
				} else if (boolValue == false) {
					continue;
				} else {
					stringValue = value as string;
				}

				if (first) {
					writer.Write (prefix);
					first = false;
				} else {
					writer.Write (attPrefix);
				}

				if (isLiteral) {
					writer.Write (stringValue ?? value);
				} else {
					WriteTo (writer, stringValue ?? value);
				}
				wroteSomething = true;
			}
			if (wroteSomething) {
				writer.Write (suffix);
			}
		}
		// This method is REQUIRED. The generated Razor subclass will override it with the generated code.
		//
		///<summary>Executes the template, writing output to the Write and WriteLiteral methods.</summary>.
		///<remarks>Not intended to be called directly. Call the Generate method instead.</remarks>
		public abstract void Execute ();

}
}
#pragma warning restore 1591
