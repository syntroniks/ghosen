using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ghosen
{
  internal class PluginLoader
  {
    public string PluginPath { get; private set; }

    // todo: enforce access, readonly collection, etc
    public IReadOnlyCollection<IPluginV1> Plugins { get; private set; }

    // Load plugins
    internal PluginLoader()
    {

    }

    internal PluginLoader(string directory)
        : base()
    {
      if (Directory.Exists(directory))
      {
        PluginPath = directory;
      }
      else
      {
        throw new DirectoryNotFoundException();
      }
    }

    internal void Reload()
    {
      FileInfo[] dllFileNames = null;
      if (Directory.Exists(PluginPath))
      {
        DirectoryInfo directory = new DirectoryInfo(PluginPath);
        dllFileNames = directory.GetFiles("*.dll");
      }
      ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
      foreach (var dllFile in dllFileNames)
      {
        Assembly assembly = Assembly.LoadFile(dllFile.FullName);
        assemblies.Add(assembly);
      }
      Type pluginType = typeof(IPluginV1);
      ICollection<Type> pluginTypes = new List<Type>();
      foreach (Assembly assembly in assemblies)
      {
        if (assembly != null)
        {
          Type[] types = assembly.GetTypes();
          foreach (Type type in types)
          {
            if (type.IsInterface || type.IsAbstract || !type.IsClass || !type.IsPublic || type.IsValueType)
            {
              continue;
            }
            else
            {
              if (type.BaseType.FullName == (pluginType.FullName))
              {
                pluginTypes.Add(type);
              }
            }
          }
        }
      }

      var pluginList = new List<IPluginV1>();
      foreach (Type type in pluginTypes)
      {
        IPluginV1 plugin = (IPluginV1)Activator.CreateInstance(type);
        pluginList.Add(plugin);
      }
      Plugins = pluginList.AsReadOnly();
    }
  }
}
