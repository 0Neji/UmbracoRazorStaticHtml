using MyNamespace.Web;
using RazorEngine;
using RazorEngine.Templating;
using RazorLight;
using System;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace UmbracoStaticHtml.Components
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class HtmlGenerationComposer : ComponentComposer<HtmlGenerationComponent>
    { }

    public class HtmlGenerationComponent : IComponent
    {
        public void Initialize()
        {
            ContentService.Saved += ContentService_Publishing;
        }

        private void ContentService_Publishing(IContentService sender, ContentSavedEventArgs e)
        {
            // RAZORLIGHT

            var engine = new RazorLightEngineBuilder()
            // required to have a default RazorLightProject type,
            // but not required to create a template from string.
            .UseEmbeddedResourcesProject(typeof(IPublishedContent))
            .SetOperatingAssembly(typeof(IPublishedContent).Assembly)
            .UseMemoryCachingProvider()
            .Build();

            var path = HttpContext.Current.Server.MapPath("~/Views/Home.cshtml");

            string key = Guid.NewGuid().ToString();

            IPublishedContent model = e.SavedEntities.First().ToPublishedContent();

            string template = File.ReadAllText(path);

            string result = engine.CompileRenderStringAsync(key, template, model).Result;

            var testststr = "";




            // RAZORENGINE

            var path2 = HttpContext.Current.Server.MapPath("~/Views/Home.cshtml");

            string key2 = Guid.NewGuid().ToString();

            IPublishedContent content = e.SavedEntities.First().ToPublishedContent();

            string templateString = File.ReadAllText(path2);

            var result2 = Engine.Razor.RunCompile(templateString, key2, model: content);

            var templateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");

            /*foreach (var templateFile in Directory.EnumerateFiles(templateFolder, "*.cshtml"))
            {
                var template = new FileInfo(templateFile);

                try
                {
                    Engine.Razor.AddTemplate(template.Name, )

                    Engine.Razor.Compile(template.Name);

                    var test2 = "";
                }
                catch (TemplateCompilationException ex)
                {
                    Console.WriteLine($"Could not pre-compile template {template.Name}", ex);

                    var test3 = "";
                }
            }*/

            var test = "";
        }

        public void Terminate()
        {
            //unsubscribe during shutdown
            //ContentService.Publishing -= ContentService_Publishing;
        }
    }
}