//  __  __       _   _       _   _ ______ _______
// |  \/  |     | | | |     | \ | |  ____|__   __|
// | \  / | __ _| |_| |__   |  \| | |__     | |
// | |\/| |/ _` | __| '_ \  | . ` |  __|    | |
// | |  | | (_| | |_| | | |_| |\  | |____   | |
// |_|  |_|\__,_|\__|_| |_(_)_| \_|______|  |_|
//
// Math.NET Numerics - https://numerics.mathdotnet.com
// Copyright (c) Math.NET - Open Source MIT/X11 License
//
// FAKE build script, see http://fsharp.github.io/FAKE
//

// --------------------------------------------------------------------------------------
// PRELUDE
// --------------------------------------------------------------------------------------

#I "packages/build/FAKE/tools"
#r "packages/build/FAKE/tools/FakeLib.dll"

open Fake
open Fake.DocuHelper
open System
open System.IO

#load "build/build-framework.fsx"
open BuildFramework


// --------------------------------------------------------------------------------------
// PROJECT INFO
// --------------------------------------------------------------------------------------

// VERSION OVERVIEW

let numericsRelease = release "Math.NET Numerics" "RELEASENOTES.md"
let mklRelease = release "MKL Provider" "RELEASENOTES-MKL.md"
let cudaRelease = release "CUDA Provider" "RELEASENOTES-CUDA.md"
let openBlasRelease = release "OpenBLAS Provider" "RELEASENOTES-OpenBLAS.md"
let dataRelease = release "Data Extensions" "RELEASENOTES-Data.md"
let releases = [ numericsRelease; mklRelease; openBlasRelease; dataRelease ] // skip cuda
traceHeader releases


// NUMERICS PACKAGES

let numericsZipPackage = zipPackage "MathNet.Numerics" "Math.NET Numerics" numericsRelease true
let numericsStrongNameZipPackage = zipPackage "MathNet.Numerics.Signed" "Math.NET Numerics" numericsRelease true

let numericsNuGetPackage = nugetPackage "MathNet.Numerics" numericsRelease
let numericsFSharpNuGetPackage = nugetPackage "MathNet.Numerics.FSharp" numericsRelease
let numericsStrongNameNuGetPackage = nugetPackage "MathNet.Numerics.Signed" numericsRelease
let numericsFSharpStrongNameNuGetPackage = nugetPackage "MathNet.Numerics.FSharp.Signed" numericsRelease

let numericsProject = project "MathNet.Numerics" "src/Numerics/Numerics.csproj" [numericsNuGetPackage; numericsStrongNameNuGetPackage]
let numericsFsharpProject = project "MathNet.Numerics.FSharp" "src/FSharp/FSharp.fsproj" [numericsFSharpNuGetPackage; numericsFSharpStrongNameNuGetPackage]
let numericsSolution = solution "Numerics" "MathNet.Numerics.sln" [numericsProject; numericsFsharpProject] [numericsZipPackage; numericsStrongNameZipPackage]


// DATA EXTENSION PACKAGES

let dataZipPackage = zipPackage "MathNet.Numerics.Data" "Math.NET Numerics Data Extensions" dataRelease false
let dataStrongNameZipPackage = zipPackage "MathNet.Numerics.Data.Signed" "Math.NET Numerics Data Extensions" dataRelease false

let dataTextNuGetPackage = nugetPackage "MathNet.Numerics.Data.Text" dataRelease
let dataMatlabNuGetPackage = nugetPackage "MathNet.Numerics.Data.Matlab" dataRelease
let dataTextStrongNameNuGetPackage = nugetPackage "MathNet.Numerics.Data.Text.Signed" dataRelease
let dataMatlabStrongNameNuGetPackage = nugetPackage "MathNet.Numerics.Data.Matlab.Signed" dataRelease

let dataTextProject = project "MathNet.Numerics.Data.Text" "src/Data/Text/Text.csproj" [dataTextNuGetPackage; dataTextStrongNameNuGetPackage]
let dataMatlabProject = project "MathNet.Numerics.Data.Matlab" "src/Data/Matlab/Matlab.csproj" [dataMatlabNuGetPackage; dataMatlabStrongNameNuGetPackage]
let dataSolution = solution "Data" "MathNet.Numerics.Data.sln" [dataTextProject; dataMatlabProject] [dataZipPackage; dataStrongNameZipPackage]


// MKL NATIVE PROVIDER PACKAGES

let mklWinPack =
    { Id = "MathNet.Numerics.MKL.Win"
      Release = mklRelease
      Title = "Math.NET Numerics - MKL Native Provider for Windows (x64 and x86)"
      Summary = ""
      Description = "Intel MKL native libraries for Math.NET Numerics on Windows."
      Tags = "math numeric statistics probability integration interpolation linear algebra matrix fft native mkl"
      Authors = [ "Christoph Ruegg"; "Marcus Cuda"; "Jurgen Van Gael" ]
      FsLoader = false
      Dependencies = []
      Files =
        [ @"..\..\build\NativeProvider.targets", Some "build\MathNet.Numerics.MKL.Win.targets", None;
          @"..\..\out\MKL\Windows\x64\libiomp5md.dll", Some @"build\x64", None;
          @"..\..\out\MKL\Windows\x64\MathNet.Numerics.MKL.dll", Some @"build\x64", None;
          @"..\..\out\MKL\Windows\x86\libiomp5md.dll", Some @"build\x86", None;
          @"..\..\out\MKL\Windows\x86\MathNet.Numerics.MKL.dll", Some @"build\x86", None ] }

let mklWin32Pack =
    { mklWinPack with
        Id = "MathNet.Numerics.MKL.Win-x86"
        Title = "Math.NET Numerics - MKL Native Provider for Windows (x86)"
        Files =
          [ @"..\..\build\NativeProvider.targets", Some "build\MathNet.Numerics.MKL.Win-x86.targets", None;
            @"..\..\out\MKL\Windows\x86\libiomp5md.dll", Some @"build\x86", None;
            @"..\..\out\MKL\Windows\x86\MathNet.Numerics.MKL.dll", Some @"build\x86", None ] }

let mklWin64Pack =
    { mklWinPack with
        Id = "MathNet.Numerics.MKL.Win-x64"
        Title = "Math.NET Numerics - MKL Native Provider for Windows (x64)"
        Files =
          [ @"..\..\build\NativeProvider.targets", Some "build\MathNet.Numerics.MKL.Win-x64.targets", None;
            @"..\..\out\MKL\Windows\x64\libiomp5md.dll", Some @"build\x64", None;
            @"..\..\out\MKL\Windows\x64\MathNet.Numerics.MKL.dll", Some @"build\x64", None ] }

let mklLinuxPack =
    { Id = "MathNet.Numerics.MKL.Linux"
      Release = mklRelease
      Title = "Math.NET Numerics - MKL Native Provider for Linux (x64 and x86)"
      Summary = ""
      Description = "Intel MKL native libraries for Math.NET Numerics on Linux."
      Tags = "math numeric statistics probability integration interpolation linear algebra matrix fft native mkl"
      Authors = [ "Christoph Ruegg"; "Marcus Cuda"; "Jurgen Van Gael" ]
      FsLoader = false
      Dependencies = []
      Files =
        [ @"..\..\build\NativeProvider.targets", Some "build\MathNet.Numerics.MKL.Linux.targets", None;
          @"..\..\out\MKL\Linux\x64\libiomp5.so", Some @"build\x64", None;
          @"..\..\out\MKL\Linux\x64\MathNet.Numerics.MKL.dll", Some @"build\x64", None;
          @"..\..\out\MKL\Linux\x86\libiomp5.so", Some @"build\x86", None;
          @"..\..\out\MKL\Linux\x86\MathNet.Numerics.MKL.dll", Some @"build\x86", None ] }

let mklLinux32Pack =
    { mklLinuxPack with
        Id = "MathNet.Numerics.MKL.Linux-x86"
        Title = "Math.NET Numerics - MKL Native Provider for Linux (x86)"
        Files =
          [ @"..\..\build\NativeProvider.targets", Some "build\MathNet.Numerics.MKL.Linux-x86.targets", None;
            @"..\..\out\MKL\Linux\x86\libiomp5.so", Some @"build\x86", None;
            @"..\..\out\MKL\Linux\x86\MathNet.Numerics.MKL.dll", Some @"build\x86", None ] }

let mklLinux64Pack =
    { mklLinuxPack with
        Id = "MathNet.Numerics.MKL.Linux-x64"
        Title = "Math.NET Numerics - MKL Native Provider for Linux (x64)"
        Files =
          [ @"..\..\build\NativeProvider.targets", Some "build\MathNet.Numerics.MKL.Linux-x64.targets", None;
            @"..\..\out\MKL\Linux\x64\libiomp5.so", Some @"build\x64", None;
            @"..\..\out\MKL\Linux\x64\MathNet.Numerics.MKL.dll", Some @"build\x64", None ] }

let mklWinBundle =
    { Id = "MathNet.Numerics.MKL.Win"
      Release = mklRelease
      Title = "Math.NET Numerics MKL Native Provider for Windows"
      Packages = [ mklWinPack; mklWin32Pack; mklWin64Pack ] }

let mklLinuxBundle =
    { Id = "MathNet.Numerics.MKL.Linux"
      Release = mklRelease
      Title = "Math.NET Numerics MKL Native Provider for Linux"
      Packages = [ mklLinuxPack; mklLinux32Pack; mklLinux64Pack ] }

let mklWinZipPackage = zipPackage "MathNet.Numerics.MKL.Win" "Math.NET Numerics MKL Native Provider for Windows" mklRelease false
let mklLinuxZipPackage = zipPackage "MathNet.Numerics.MKL.Linux" "Math.NET Numerics MKL Native Provider for Linux" mklRelease false

let mklWinNuGetPackage = nugetPackage "MathNet.Numerics.MKL.Win" mklRelease
let mklWin32NuGetPackage = nugetPackage "MathNet.Numerics.MKL.Win-x86" mklRelease
let mklWin64NuGetPackage = nugetPackage "MathNet.Numerics.MKL.Win-x64" mklRelease
let mklLinuxNuGetPackage = nugetPackage "MathNet.Numerics.MKL.Linux" mklRelease
let mklLinux32NuGetPackage = nugetPackage "MathNet.Numerics.MKL.Linux-x86" mklRelease
let mklLinux64NuGetPackage = nugetPackage "MathNet.Numerics.MKL.Linux-x64" mklRelease


// CUDA NATIVE PROVIDER PACKAGES

let cudaWinPack =
    { Id = "MathNet.Numerics.CUDA.Win"
      Release = cudaRelease
      Title = "Math.NET Numerics - CUDA Native Provider for Windows (x64)"
      Summary = ""
      Description = "Nvidia CUDA native libraries for Math.NET Numerics."
      Tags = "math numeric statistics probability integration interpolation linear algebra matrix fft native cuda gpu"
      Authors = [ "Matthew A Johnson"; "Christoph Ruegg" ]
      FsLoader = false
      Dependencies = []
      Files =
        [ @"..\..\build\NativeProvider.targets", Some "build\MathNet.Numerics.CUDA.Win.targets", None;
          @"..\..\out\CUDA\Windows\x64\cublas64_70.dll", Some "content", None;
          @"..\..\out\CUDA\Windows\x64\cudart64_70.dll", Some "content", None;
          @"..\..\out\CUDA\Windows\x64\cusolver64_70.dll", Some "content", None;
          @"..\..\out\CUDA\Windows\x64\MathNet.Numerics.CUDA.dll", Some "content", None ] }

let cudaWinBundle =
    { Id = "MathNet.Numerics.CUDA.Win"
      Release = cudaRelease
      Title = "Math.NET Numerics CUDA Native Provider for Windows"
      Packages = [ cudaWinPack ] }

let cudaWinZipPackage = zipPackage "MathNet.Numerics.CUDA.Win" "Math.NET Numerics CUDA Native Provider for Windows" cudaRelease false
let cudaWinNuGetPackage = nugetPackage "MathNet.Numerics.CUDA.Win" cudaRelease


// OpenBLAS NATIVE PROVIDER PACKAGES

let openBlasWinPack =
    { Id = "MathNet.Numerics.OpenBLAS.Win"
      Release = openBlasRelease
      Title = "Math.NET Numerics - OpenBLAS Native Provider for Windows (x64 and x86)"
      Summary = ""
      Description = "OpenBLAS native libraries for Math.NET Numerics."
      Tags = "math numeric statistics probability integration interpolation linear algebra matrix fft native openblas"
      Authors = [ "Kuan Bartel"; "Christoph Ruegg"; "Marcus Cuda" ]
      FsLoader = false
      Dependencies = []
      Files =
        [ @"..\..\build\NativeProvider.targets", Some "build\MathNet.Numerics.OpenBLAS.Win.targets", None;
          @"..\..\out\OpenBLAS\Windows\x64\libgcc_s_seh-1.dll", Some @"build\x64", None;
          @"..\..\out\OpenBLAS\Windows\x64\libgfortran-3.dll", Some @"build\x64", None;
          @"..\..\out\OpenBLAS\Windows\x64\libopenblas.dll", Some @"build\x64", None;
          @"..\..\out\OpenBLAS\Windows\x64\libquadmath-0.dll", Some @"build\x64", None;
          @"..\..\out\OpenBLAS\Windows\x64\MathNet.Numerics.OpenBLAS.dll", Some @"build\x64", None;
          @"..\..\out\OpenBLAS\Windows\x86\libgcc_s_sjlj-1.dll", Some @"build\x86", None;
          @"..\..\out\OpenBLAS\Windows\x86\libgfortran-3.dll", Some @"build\x86", None;
          @"..\..\out\OpenBLAS\Windows\x86\libopenblas.dll", Some @"build\x86", None;
          @"..\..\out\OpenBLAS\Windows\x86\libquadmath-0.dll", Some @"build\x86", None;
          @"..\..\out\OpenBLAS\Windows\x86\MathNet.Numerics.OpenBLAS.dll", Some @"build\x86", None ] }

let openBlasWinBundle =
    { Id = "MathNet.Numerics.OpenBLAS.Win"
      Release = openBlasRelease
      Title = "Math.NET Numerics OpenBLAS Native Provider for Windows"
      Packages = [ openBlasWinPack ] }

let openBlasWinZipPackage = zipPackage "MathNet.Numerics.OpenBLAS.Win" "Math.NET Numerics OpenBLAS Native Provider for Windows" openBlasRelease false
let openBlasWinNuGetPackage = nugetPackage "MathNet.Numerics.OpenBLAS.Win" openBlasRelease


// ALL

let allSolutions = [numericsSolution; dataSolution]
let allProjects = allSolutions |> List.collect (fun s -> s.Projects) |> List.distinct


// --------------------------------------------------------------------------------------
// PREPARE
// --------------------------------------------------------------------------------------

Target "Start" DoNothing

Target "Clean" (fun _ ->
    DeleteDirs (!! "src/**/obj/" ++ "src/**/bin/" )
    CleanDirs [ "out/api"; "out/docs" ]
    CleanDirs [ "out/MKL"; "out/ATLAS"; "out/CUDA"; "out/OpenBLAS" ] // Native Providers
    allSolutions |> List.iter (fun solution -> CleanDirs [ solution.OutputZipDir; solution.OutputNuGetDir; solution.OutputLibDir; solution.OutputLibStrongNameDir ])
    allSolutions |> List.iter clean)

Target "ApplyVersion" (fun _ ->
    allProjects |> List.iter patchVersionInProjectFile
    patchVersionInAssemblyInfo "src/FSharp" numericsRelease
    patchVersionInAssemblyInfo "src/TestData" numericsRelease
    patchVersionInAssemblyInfo "src/Numerics.Tests" numericsRelease
    patchVersionInAssemblyInfo "src/FSharp.Tests" numericsRelease
    patchVersionInAssemblyInfo "src/Data.Tests" dataRelease
    patchVersionInResource "src/NativeProviders/MKL/resource.rc" mklRelease
    patchVersionInResource "src/NativeProviders/CUDA/resource.rc" cudaRelease
    patchVersionInResource "src/NativeProviders/OpenBLAS/resource.rc" openBlasRelease)

Target "Restore" (fun _ -> allSolutions |> List.iter restore)
"Start"
  =?> ("Clean", not (hasBuildParam "incremental"))
  ==> "Restore"

Target "Prepare" DoNothing
"Start"
  =?> ("Clean", not (hasBuildParam "incremental"))
  ==> "ApplyVersion"
  ==> "Prepare"


// --------------------------------------------------------------------------------------
// BUILD, SIGN, COLLECT
// --------------------------------------------------------------------------------------

let fingerprint = "5dbea70701b40cab1b2ca62c75401342b4f0f03a"
let timeserver = "http://time.certum.pl/"

Target "Build" (fun _ ->

    // Strong Name Build (with strong name, without certificate signature)
    if hasBuildParam "strongname" then
        CleanDirs (!! "src/**/obj/" ++ "src/**/bin/" )
        restoreSN numericsSolution
        buildSN numericsSolution
        collectBinariesSN numericsSolution
        zip numericsStrongNameZipPackage numericsSolution.OutputZipDir numericsSolution.OutputLibStrongNameDir (fun f -> f.Contains("MathNet.Numerics.") || f.Contains("System.Threading.") || f.Contains("FSharp.Core."))
        if isWindows then
            packSN numericsSolution.SolutionFile
            collectNuGetPackages numericsSolution

    // Normal Build (without strong name, with certificate signature)
    CleanDirs (!! "src/**/obj/" ++ "src/**/bin/" )
    restore numericsSolution
    build numericsSolution
    if isWindows && hasBuildParam "sign" then sign fingerprint timeserver numericsSolution
    collectBinaries numericsSolution
    zip numericsZipPackage numericsSolution.OutputZipDir numericsSolution.OutputLibDir (fun f -> f.Contains("MathNet.Numerics.") || f.Contains("System.Threading.") || f.Contains("FSharp.Core."))
    if isWindows then
        pack numericsSolution.SolutionFile
        collectNuGetPackages numericsSolution

    // NuGet Sign (all or nothing)
    if isWindows && hasBuildParam "sign" then signNuGet fingerprint timeserver numericsSolution

    )
"Prepare" ==> "Build"

Target "DataBuild" (fun _ ->

    // Strong Name Build (with strong name, without certificate signature)
    if hasBuildParam "strongname" then
        CleanDirs (!! "src/**/obj/" ++ "src/**/bin/" )
        restoreSN dataSolution
        buildSN dataSolution
        collectBinariesSN dataSolution
        zip dataStrongNameZipPackage dataSolution.OutputZipDir dataSolution.OutputLibStrongNameDir (fun f -> f.Contains("MathNet.Numerics.Data."))
        if isWindows then
            packSN dataTextProject.ProjectFile
            packSN dataMatlabProject.ProjectFile
            collectNuGetPackages dataSolution

    // Normal Build (without strong name, with certificate signature)
    CleanDirs (!! "src/**/obj/" ++ "src/**/bin/" )
    restore dataSolution
    build dataSolution
    if isWindows && hasBuildParam "sign" then sign fingerprint timeserver dataSolution
    collectBinaries dataSolution
    zip dataZipPackage dataSolution.OutputZipDir dataSolution.OutputLibDir (fun f -> f.Contains("MathNet.Numerics.Data."))
    if isWindows then
        pack dataTextProject.ProjectFile
        pack dataMatlabProject.ProjectFile
        collectNuGetPackages dataSolution

    // NuGet Sign (all or nothing)
    if isWindows && hasBuildParam "sign" then signNuGet fingerprint timeserver dataSolution

    )
"Prepare" ==> "DataBuild"

Target "MklWin32Build" (fun _ -> buildConfig32 "Release-MKL" !! "MathNet.Numerics.NativeProviders.sln")
Target "MklWin64Build" (fun _ -> buildConfig64 "Release-MKL" !! "MathNet.Numerics.NativeProviders.sln")
Target "MklWinBuild" DoNothing
"Prepare" ==> "MklWin32Build" ==> "MklWinBuild"
"Prepare" ==> "MklWin64Build" ==> "MklWinBuild"

Target "CudaWin64Build" (fun _ -> buildConfig64 "Release-CUDA" !! "MathNet.Numerics.NativeProviders.sln")
Target "CudaWinBuild" DoNothing
"Prepare" ==> "CudaWin64Build" ==> "CudaWinBuild"

Target "OpenBlasWin32Build" (fun _ -> buildConfig32 "Release-OpenBLAS" !! "MathNet.Numerics.NativeProviders.sln")
Target "OpenBlasWin64Build" (fun _ -> buildConfig64 "Release-OpenBLAS" !! "MathNet.Numerics.NativeProviders.sln")
Target "OpenBlasWinBuild" DoNothing
"Prepare" ==> "OpenBlasWin32Build" ==> "OpenBlasWinBuild"
"Prepare" ==> "OpenBlasWin64Build" ==> "OpenBlasWinBuild"


// --------------------------------------------------------------------------------------
// TEST
// --------------------------------------------------------------------------------------

let testNumerics framework = test "src/Numerics.Tests" "Numerics.Tests.csproj" framework
Target "TestNumerics" DoNothing
Target "TestNumericsCore1.1" (fun _ -> testNumerics "netcoreapp1.1")
Target "TestNumericsCore2.0" (fun _ -> testNumerics "netcoreapp2.0")
Target "TestNumericsNET40" (fun _ -> testNumerics "net40")
Target "TestNumericsNET45" (fun _ -> testNumerics "net45")
Target "TestNumericsNET461" (fun _ -> testNumerics "net461")
Target "TestNumericsNET47"  (fun _ -> testNumerics "net47")
"Build" ==> "TestNumericsCore1.1"
"Build" ==> "TestNumericsCore2.0" ==> "TestNumerics"
"Build" =?> ("TestNumericsNET40", isWindows)
"Build" =?> ("TestNumericsNET45", isWindows)
"Build" =?> ("TestNumericsNET461", isWindows) ==> "TestNumerics"
"Build" =?> ("TestNumericsNET47", isWindows)
let testFsharp framework = test "src/FSharp.Tests" "FSharp.Tests.fsproj" framework
Target "TestFsharp" DoNothing
Target "TestFsharpCore1.1" (fun _ -> testFsharp "netcoreapp1.1")
Target "TestFsharpCore2.0" (fun _ -> testFsharp "netcoreapp2.0")
Target "TestFsharpNET45" (fun _ -> testFsharp "net45")
Target "TestFsharpNET461" (fun _ -> testFsharp "net461")
Target "TestFsharpNET47" (fun _ -> testFsharp "net47")
"Build" ==> "TestFsharpCore1.1"
"Build" ==> "TestFsharpCore2.0" ==> "TestFsharp"
"Build" =?> ("TestFsharpNET45", isWindows)
"Build" =?> ("TestFsharpNET461", isWindows) ==> "TestFsharp"
"Build" =?> ("TestFsharpNET47", isWindows)
Target "Test" DoNothing
"TestNumerics" ==> "Test"
"TestFsharp" ==> "Test"

let testMKL framework = test "src/Numerics.Tests" "Numerics.Tests.MKL.csproj" framework
Target "MklTest" DoNothing
Target "MklTestCore2.0" (fun _ -> testMKL "netcoreapp2.0")
Target "MklTestNET40" (fun _ -> testMKL "net40")
"Build" ==> "MklTestCore2.0" ==> "MklTest"
"Build" =?> ("MklTestNET40", isWindows) ==> "MklTest"

let testOpenBLAS framework = test "src/Numerics.Tests" "Numerics.Tests.OpenBLAS.csproj" framework
Target "OpenBlasTest" DoNothing
Target "OpenBlasTestCore2.0" (fun _ -> testOpenBLAS "netcoreapp2.0")
Target "OpenBlasTestNET40" (fun _ -> testOpenBLAS "net40")
"Build" ==> "OpenBlasTestCore2.0" ==> "OpenBlasTest"
"Build" =?> ("OpenBlasTestNET40", isWindows) ==> "OpenBlasTest"

let testCUDA framework = test "src/Numerics.Tests" "Numerics.Tests.CUDA.csproj" framework
Target "CudaTest" DoNothing
Target "CudaTestCore2.0" (fun _ -> testCUDA "netcoreapp2.0")
Target "CudaTestNET40" (fun _ -> testCUDA "net40")
"Build" ==> "CudaTestCore2.0" ==> "CudaTest"
"Build" =?> ("CudaTestNET40", isWindows) ==> "CudaTest"

let testData framework = test "src/Data.Tests" "Data.Tests.csproj" framework
Target "DataTest" DoNothing
Target "DataTestCore1.1" (fun _ -> testData "netcoreapp1.1")
Target "DataTestCore2.0" (fun _ -> testData "netcoreapp2.0")
Target "DataTestNET45" (fun _ -> testData "net45")
"DataBuild" ==> "DataTestCore1.1"
"DataBuild" ==> "DataTestCore2.0" ==> "DataTest"
"DataBuild" =?> ("DataTestNET45", isWindows) ==> "DataTest"


// --------------------------------------------------------------------------------------
// PACKAGES
// --------------------------------------------------------------------------------------

Target "MklWinPack" DoNothing
Target "MklLinuxPack" DoNothing
Target "CudaWinPack" DoNothing
Target "OpenBlasWinPack" DoNothing


// ZIP

Target "MklWinZip" (fun _ ->
    CreateDir "out/MKL/packages/Zip"
    zip mklWinZipPackage "out/MKL/packages/Zip" "out/MKL/Windows" (fun f -> f.Contains("MathNet.Numerics.MKL.") || f.Contains("libiomp5md.dll")))
"MklWinBuild" ==> "MklWinZip" ==> "MklWinPack"

Target "MklLinuxZip" (fun _ ->
    CreateDir "out/MKL/packages/Zip"
    zip mklLinuxZipPackage "out/MKL/packages/Zip" "out/MKL/Linux" (fun f -> f.Contains("MathNet.Numerics.MKL.") || f.Contains("libiomp5.so")))
// "MklLinuxBuild" ==> "MklLinuxZip" ==> "MklLinuxPack"
"MklLinuxZip" ==> "MklLinuxPack"

Target "CudaWinZip" (fun _ ->
    CreateDir "out/CUDA/packages/Zip"
    zip cudaWinZipPackage "out/CUDA/packages/Zip" "out/CUDA/Windows" (fun f -> f.Contains("MathNet.Numerics.CUDA.") || f.Contains("cublas") || f.Contains("cudart") || f.Contains("cusolver")))
"CudaWinBuild" ==> "CudaWinZip" ==> "CudaWinPack"

Target "OpenBlasWinZip" (fun _ ->
    CreateDir "out/OpenBLAS/packages/Zip"
    zip openBlasWinZipPackage "out/OpenBLAS/packages/Zip" "out/OpenBLAS/Windows" (fun f -> f.Contains("MathNet.Numerics.OpenBLAS.") || f.Contains("libgcc") || f.Contains("libgfortran") || f.Contains("libopenblas") || f.Contains("libquadmath")))
"OpenBlasWinBuild" ==> "OpenBlasWinZip" ==> "OpenBlasWinPack"

// NUGET

Target "MklWinNuGet" (fun _ ->
    CreateDir "out/MKL/packages/NuGet"
    nugetPackExtension mklWinBundle "out/MKL/packages/NuGet")
"MklWinBuild" ==> "MklWinNuGet" ==> "MklWinPack"

Target "MklLinuxNuGet" (fun _ ->
    CreateDir "out/MKL/packages/NuGet"
    nugetPackExtension mklLinuxBundle "out/MKL/packages/NuGet")
// "MklLinuxBuild" ==> "MklLinuxNuGet" ==> "MklLinuxPack"
"MklLinuxNuGet" ==> "MklLinuxPack"

Target "CudaWinNuGet" (fun _ ->
    CreateDir "out/CUDA/packages/NuGet"
    nugetPackExtension cudaWinBundle "out/CUDA/packages/NuGet")
"CudaWinBuild" ==> "CudaWinNuGet" ==> "CudaWinPack"

Target "OpenBlasWinNuGet" (fun _ ->
    CreateDir "out/OpenBLAS/packages/NuGet"
    nugetPackExtension openBlasWinBundle "out/OpenBLAS/packages/NuGet")
"OpenBlasWinBuild" ==> "OpenBlasWinNuGet" ==> "OpenBlasWinPack"


// --------------------------------------------------------------------------------------
// Documentation
// --------------------------------------------------------------------------------------

// DOCS

Target "CleanDocs" (fun _ -> CleanDirs ["out/docs"])

let extraDocs =
    [ "LICENSE.md", "License.md"
      "CONTRIBUTING.md", "Contributing.md"
      "CONTRIBUTORS.md", "Contributors.md" ]

Target "Docs" (fun _ ->
    provideDocExtraFiles extraDocs releases
    generateDocs true false)
Target "DocsDev" (fun _ ->
    provideDocExtraFiles  extraDocs releases
    generateDocs true true)
Target "DocsWatch" (fun _ ->
    provideDocExtraFiles  extraDocs releases
    use watcher = new FileSystemWatcher(DirectoryInfo("docs/content").FullName, "*.*")
    watcher.EnableRaisingEvents <- true
    watcher.Changed.Add(fun e -> generateDocs false true)
    watcher.Created.Add(fun e -> generateDocs false true)
    watcher.Renamed.Add(fun e -> generateDocs false true)
    watcher.Deleted.Add(fun e -> generateDocs false true)
    traceImportant "Waiting for docs edits. Press any key to stop."
    System.Console.ReadKey() |> ignore
    watcher.EnableRaisingEvents <- false
    watcher.Dispose())

"Build" ==> "CleanDocs" ==> "Docs"

"Start"
  =?> ("CleanDocs", not (hasBuildParam "incremental"))
  ==> "DocsDev"
  ==> "DocsWatch"


// API REFERENCE

Target "CleanApi" (fun _ -> CleanDirs ["out/api"])

Target "Api" (fun _ ->
    !! "src/Numerics/bin/Release/net40/MathNet.Numerics.dll"
    |> Docu (fun p ->
        { p with
            ToolPath = "tools/docu/docu.exe"
            TemplatesPath = "tools/docu/templates/"
            TimeOut = TimeSpan.FromMinutes 10.
            OutputPath = "out/api/" }))

"Build" ==> "CleanApi" ==> "Api"


// --------------------------------------------------------------------------------------
// Publishing
// Requires permissions; intended only for maintainers
// --------------------------------------------------------------------------------------

Target "PublishTag" (fun _ -> publishReleaseTag "Math.NET Numerics" "" numericsRelease)
Target "DataPublishTag" (fun _ -> publishReleaseTag "Math.NET Numerics Data Extensions" "data-" dataRelease)
Target "MklPublishTag" (fun _ -> publishReleaseTag "Math.NET Numerics MKL Provider" "mkl-" mklRelease)
Target "CudaPublishTag" (fun _ -> publishReleaseTag "Math.NET Numerics CUDA Provider" "cuda-" cudaRelease)
Target "OpenBlasPublishTag" (fun _ -> publishReleaseTag "Math.NET Numerics OpenBLAS Provider" "openblas-" openBlasRelease)

Target "PublishMirrors" (fun _ -> publishMirrors ())
Target "PublishDocs" (fun _ -> publishDocs numericsRelease)
Target "PublishApi" (fun _ -> publishApi numericsRelease)

Target "PublishArchive" (fun _ -> publishArchive numericsSolution)
Target "DataPublishArchive" (fun _ -> publishArchive dataSolution)
Target "MklPublishArchive" (fun _ -> publishArchiveManual "out/MKL/packages/Zip" "out/MKL/packages/NuGet" [mklWinZipPackage; mklLinuxZipPackage] [mklWinNuGetPackage; mklWin32NuGetPackage; mklWin64NuGetPackage; mklLinuxNuGetPackage; mklLinux32NuGetPackage; mklLinux64NuGetPackage])
Target "CudaPublishArchive" (fun _ -> publishArchiveManual "out/CUDA/packages/Zip" "out/CUDA/packages/NuGet" [cudaWinZipPackage] [cudaWinNuGetPackage])
Target "OpenBlasPublishArchive" (fun _ -> publishArchiveManual "out/OpenBLAS/packages/Zip" "out/OpenBLAS/packages/NuGet" [openBlasWinZipPackage] [openBlasWinNuGetPackage])

Target "PublishNuGet" (fun _ -> publishNuGet !! (numericsSolution.OutputNuGetDir </> "/*.nupkg"))
Target "DataPublishNuGet" (fun _ -> publishNuGet !! (dataSolution.OutputNuGetDir </> "/*.nupkg"))
Target "MklPublishNuGet" (fun _ -> publishNuGet !! "out/MKL/packages/NuGet/*.nupkg")
Target "CudaPublishNuGet" (fun _ -> publishNuGet !! "out/CUDA/packages/NuGet/*.nupkg")
Target "OpenBlasPublishNuGet" (fun _ -> publishNuGet !! "out/OpenBLAS/packages/NuGet/*.nupkg")

Target "Publish" DoNothing
Dependencies "Publish" [ "PublishTag"; "PublishDocs"; "PublishApi"; "PublishArchive"; "PublishNuGet" ]

Target "DataPublish" DoNothing
Dependencies "DataPublish" [ "DataPublishTag"; "DataPublishArchive"; "DataPublishNuGet" ]

Target "MklPublish" DoNothing
Dependencies "MklPublish" [ "MklPublishTag"; "PublishDocs"; "MklPublishArchive"; "MklPublishNuGet" ]

Target "CudaPublish" DoNothing
Dependencies "CudaPublish" [ "CudaPublishTag"; "PublishDocs"; "CudaPublishArchive"; "CudaPublishNuGet" ]

Target "OpenBlasPublish" DoNothing
Dependencies "OpenBlasPublish" [ "OpenBlasPublishTag"; "PublishDocs"; "OpenBlasPublishArchive"; "OpenBlasPublishNuGet" ]


// --------------------------------------------------------------------------------------
// Default Targets
// --------------------------------------------------------------------------------------

Target "All" DoNothing
Dependencies "All" [ "Build"; "Docs"; "Api"; "Test" ]

Target "DataAll" DoNothing
Dependencies "DataAll" [ "DataBuild"; "DataTest" ]

Target "MklWinAll" DoNothing
Dependencies "MklWinAll" [ "MklWinPack"; "MklTest" ]

Target "CudaWinAll" DoNothing
Dependencies "CudaWinAll" [ "CudaWinPack"; "CudaTest" ]

Target "OpenBlasWinAll" DoNothing
Dependencies "OpenBlasWinAll" [ "OpenBlasWinPack"; "OpenBlasTest" ]

RunTargetOrDefault "Test"
