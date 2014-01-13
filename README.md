vvvv-Template
=============

Scaffolding VVVV Pack structure 

An early decision to push logic out of nodes and into the core is essential when you use custom link types between your nodes. If you move the custom type to the core, you and other people can use its dll just like an API.

Introduction
============

This Template helps you kickstart a new Pack for [vvvv](http://www.vvvv.org). 


Its structure has been deduced from the very few packages that exist now, but I hope to encourage broader adoption with this contribution. 

You can start your new Pack with a fresh download. Take these steps 

1. Change the name of the directory to the name of your pack, but keep the vvvv- in front of it. This will help to identify packs quickly
2. Change the name of the solution file in `/src` the same. Lets refer to the name now as vvvv-YourSolutionName
3. Startup your IDE with the solution file
4. In the Solution Explorer you can Rename the two contained projects
5. in `Build`->`Configuration Manager...` you can define your Build profile. As of now pick either x86 or x64. If you do not plan to debug, set it to Release. 
6. Rebuild solution. 
7. Locate the `/vvvv-YourSolutionName` in `/build` according to your previous settings. Copy that folder to your `/packs` in your vvvv installation 
8. Test with patching a node named `TemplateTemplate`. of course you will want to delete this plugin in the Solution Explorer quickly :)


Troubleshooting
============

- If you are having problems with nuget auto restore packages, try this 
  1. download [nuget.exe](http://download-codeplex.sec.s-msft.com/Download/Release?ProjectName=nuget&DownloadId=757017&FileTime=130290366297630000&Build=20841)
  2. add nuget's path to the windows Path environment variable.
  3. do from cmd in the `/vvvv-YourSolutionName/src`: 
     `nuget.exe update -self` 
     `nuget restore` 

- If you are sick of copying files around:
  1. goto your vvvv distribution `packs` with cmd
  2. adjust this command to your setup: `mklink /J vvvv-YourSolutionName "c:\dev\vvvv-YourSolutionName\build\x64\Debug\vvvv-YourSolutionName"`
  3. alternatively take a look in the Nodes.csproj where you can add 'AfterBuild' tasks for more advanced deployment 

License
=======

Marko Ritter (www.intolight.com)

This software is distributed under the [MIT license](http://opensource.org/licenses/MIT)

Feel free to use and improve this in any way, and allow yourself to contribute too
