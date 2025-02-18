```shell
USAGE:
    trx [OPTIONS]

OPTIONS:
                          DEFAULT                                               
    -h, --help                       Prints help information                    
    -v, --version                    Prints version information                 
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
