![Icon](assets/icon.png) dotnet-trx
============

[![Version](https://img.shields.io/nuget/vpre/dotnet-trx.svg?color=royalblue)](https://www.nuget.org/packages/dotnet-trx)
[![Downloads](https://img.shields.io/nuget/dt/dotnet-trx.svg?color=green)](https://www.nuget.org/packages/dotnet-trx)
[![License](https://img.shields.io/github/license/devlooped/dotnet-trx.svg?color=blue)](https://github.com//devlooped/dotnet-trx/blob/main/license.txt)
[![Build](https://github.com/devlooped/dotnet-trx/workflows/build/badge.svg?branch=main)](https://github.com/devlooped/dotnet-trx/actions)

<!-- #content -->
Pretty-print test results in TRX format.

![Demo](https://raw.githubusercontent.com/devlooped/dotnet-trx/main/assets/img/demo.png)

Typical usage: `dotnet test --logger trx; trx` (optionally with `--output`).

It automatically integrates with GitHub Actions by appending a pull request comment with the results too.
Set up workflow with:

```yml
    - name: 🧪 show
      continue-on-error: true
      if: always()
      run:  
        dotnet tool update -g dotnet-trx
        trx --output
```

And view results in an automatic pull request comment like:

![PR comment](https://raw.githubusercontent.com/devlooped/dotnet-trx/main/assets/img/comment.png)

> NOTE: this behavior is triggered by the presence of the `GITHUB_REF_NAME` and `CI` environment variables.

<!-- include src/dotnet-trx/help.md -->
```shell
USAGE:
    trx [OPTIONS]

OPTIONS:
                          DEFAULT                                               
    -h, --help                       Prints help information                    
        --version                    Prints version information                 
    -p, --path                       Optional base directory for *.trx files    
                                     discovery. Defaults to current directory   
    -o, --output                     Include test output                        
    -r, --recursive       True       Recursively search for *.trx files         
    -v, --verbosity       Quiet      Output display verbosity:                  
                                     - quiet: only failed tests are displayed   
                                     - normal: failed and skipped tests are     
                                     displayed                                  
                                     - verbose: failed, skipped and passed tests
                                     are displayed                              
        --no-exit-code               Do not return a -1 exit code on test       
                                     failures                                   
        --gh-comment      True       Report as GitHub PR comment                
        --gh-summary      True       Report as GitHub step summary              
```

<!-- src/dotnet-trx/help.md -->

Install:

```shell
dotnet tool install -g dotnet-trx
```

Update:

```shell
dotnet tool update -g dotnet-trx
```

<!-- #content -->

## Open Source Maintenance Fee

To ensure the long-term sustainability of this project, use of dotnet-trx requires an 
[Open Source Maintenance Fee](https://opensourcemaintenancefee.org). While the source 
code is freely available under the terms of the [MIT License](https://github.com/devlooped/dotnet-trx/blob/main/license.txt), all other aspects of the 
project --including opening or commenting on issues, participating in discussions and 
downloading releases-- require [adherence to the Maintenance Fee](https://github.com/devlooped/dotnet-trx/blob/main/osmfeula.txt).

In short, if you use this project to generate revenue, the [Maintenance Fee is required](https://github.com/devlooped/dotnet-trx/blob/main/osmfeula.txt).

To pay the Maintenance Fee, [become a Sponsor](https://github.com/sponsors/devlooped) at the corresponding OSMF tier (starting at just $10!).

<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
# Sponsors 

<!-- sponsors.md -->
[![Clarius Org](https://avatars.githubusercontent.com/u/71888636?v=4&s=39 "Clarius Org")](https://github.com/clarius)
[![MFB Technologies, Inc.](https://avatars.githubusercontent.com/u/87181630?v=4&s=39 "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![SandRock](https://avatars.githubusercontent.com/u/321868?u=99e50a714276c43ae820632f1da88cb71632ec97&v=4&s=39 "SandRock")](https://github.com/sandrock)
[![DRIVE.NET, Inc.](https://avatars.githubusercontent.com/u/15047123?v=4&s=39 "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Keith Pickford](https://avatars.githubusercontent.com/u/16598898?u=64416b80caf7092a885f60bb31612270bffc9598&v=4&s=39 "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://avatars.githubusercontent.com/u/127185?u=7f50babfc888675e37feb80851a4e9708f573386&v=4&s=39 "Thomas Bolon")](https://github.com/tbolon)
[![Reuben Swartz](https://avatars.githubusercontent.com/u/724704?u=2076fe336f9f6ad678009f1595cbea434b0c5a41&v=4&s=39 "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://avatars.githubusercontent.com/u/480334?v=4&s=39 "Jacob Foshee")](https://github.com/jfoshee)
[![](https://avatars.githubusercontent.com/u/33566379?u=bf62e2b46435a267fa246a64537870fd2449410f&v=4&s=39 "")](https://github.com/Mrxx99)
[![Eric Johnson](https://avatars.githubusercontent.com/u/26369281?u=41b560c2bc493149b32d384b960e0948c78767ab&v=4&s=39 "Eric Johnson")](https://github.com/eajhnsn1)
[![Jonathan ](https://avatars.githubusercontent.com/u/5510103?u=98dcfbef3f32de629d30f1f418a095bf09e14891&v=4&s=39 "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Ken Bonny](https://avatars.githubusercontent.com/u/6417376?u=569af445b6f387917029ffb5129e9cf9f6f68421&v=4&s=39 "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://avatars.githubusercontent.com/u/122666?v=4&s=39 "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://avatars.githubusercontent.com/u/5989304?v=4&s=39 "agileworks-eu")](https://github.com/agileworks-eu)
[![Zheyu Shen](https://avatars.githubusercontent.com/u/4067473?v=4&s=39 "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://avatars.githubusercontent.com/u/87844133?v=4&s=39 "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://avatars.githubusercontent.com/u/16239022?v=4&s=39 "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://avatars.githubusercontent.com/u/68428092?v=4&s=39 "4OTC")](https://github.com/4OTC)
[![domischell](https://avatars.githubusercontent.com/u/66068846?u=0a5c5e2e7d90f15ea657bc660f175605935c5bea&v=4&s=39 "domischell")](https://github.com/DominicSchell)
[![Adrian Alonso](https://avatars.githubusercontent.com/u/2027083?u=129cf516d99f5cb2fd0f4a0787a069f3446b7522&v=4&s=39 "Adrian Alonso")](https://github.com/adalon)
[![torutek](https://avatars.githubusercontent.com/u/33917059?v=4&s=39 "torutek")](https://github.com/torutek)
[![Ryan McCaffery](https://avatars.githubusercontent.com/u/16667079?u=c0daa64bb5c1b572130e05ae2b6f609ecc912d4d&v=4&s=39 "Ryan McCaffery")](https://github.com/mccaffers)
[![Seika Logiciel](https://avatars.githubusercontent.com/u/2564602?v=4&s=39 "Seika Logiciel")](https://github.com/SeikaLogiciel)
[![Andrew Grant](https://avatars.githubusercontent.com/devlooped-user?s=39 "Andrew Grant")](https://github.com/wizardness)
[![eska-gmbh](https://avatars.githubusercontent.com/devlooped-team?s=39 "eska-gmbh")](https://github.com/eska-gmbh)
[![Geodata AS](https://avatars.githubusercontent.com/u/5946299?v=4&s=39 "Geodata AS")](https://github.com/geodata-no)
[![Jiri Slachta](https://avatars.githubusercontent.com/u/6891947?u=802cfeb13b070d04c53269fc662b0d58963480dd&v=4&s=39 "Jiri Slachta")](https://github.com/jslachta)


<!-- sponsors.md -->
[![Sponsor this project](https://avatars.githubusercontent.com/devlooped-sponsor?s=118 "Sponsor this project")](https://github.com/sponsors/devlooped)

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
