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
[![Clarius Org](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/clarius.png "Clarius Org")](https://github.com/clarius)
[![MFB Technologies, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MFB-Technologies-Inc.png "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![DRIVE.NET, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/drivenet.png "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Keith Pickford](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Keflon.png "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tbolon.png "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/kfrancis.png "Kori Francis")](https://github.com/kfrancis)
[![Toni Wenzel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/twenzel.png "Toni Wenzel")](https://github.com/twenzel)
[![Uno Platform](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/unoplatform.png "Uno Platform")](https://github.com/unoplatform)
[![Reuben Swartz](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/rbnswartz.png "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jfoshee.png "Jacob Foshee")](https://github.com/jfoshee)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Mrxx99.png "")](https://github.com/Mrxx99)
[![Eric Johnson](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/eajhnsn1.png "Eric Johnson")](https://github.com/eajhnsn1)
[![David JENNI](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/davidjenni.png "David JENNI")](https://github.com/davidjenni)
[![Jonathan ](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Jonathan-Hickey.png "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Charley Wu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/akunzai.png "Charley Wu")](https://github.com/akunzai)
[![Ken Bonny](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KenBonny.png "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/SimonCropp.png "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/agileworks-eu.png "agileworks-eu")](https://github.com/agileworks-eu)
[![Zheyu Shen](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/arsdragonfly.png "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/vezel-dev.png "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/ChilliCream.png "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/4OTC.png "4OTC")](https://github.com/4OTC)
[![Vincent Limo](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/v-limo.png "Vincent Limo")](https://github.com/v-limo)
[![Jordan S. Jones](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jordansjones.png "Jordan S. Jones")](https://github.com/jordansjones)
[![domischell](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/DominicSchell.png "domischell")](https://github.com/DominicSchell)
[![Justin Wendlandt](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jwendl.png "Justin Wendlandt")](https://github.com/jwendl)
[![Adrian Alonso](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/adalon.png "Adrian Alonso")](https://github.com/adalon)
[![Michael Hagedorn](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Eule02.png "Michael Hagedorn")](https://github.com/Eule02)
[![torutek](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/torutek.png "torutek")](https://github.com/torutek)
[![Ryan McCaffery](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/mccaffers.png "Ryan McCaffery")](https://github.com/mccaffers)
[![Alex Wiese](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/alexwiese.png "Alex Wiese")](https://github.com/alexwiese)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
