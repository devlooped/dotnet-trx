```shell
USAGE:
    trx [OPTIONS]

OPTIONS:
                          DEFAULT                                               
    -h, --help                       Prints help information                    
    -p, --path                       Optional base directory for *.trx files    
                                     discovery. Defaults to current directory   
    -o, --output                     Include test output                        
    -r, --recursive       [1mTrue[0m       Recursively search for *.trx files         
        --skipped         [1mTrue[0m       Include skipped tests                      
        --no-exit-code               Do not return a -1 exit code on test       
                                     failures                                   
        --version                    Show version information                   
        --gh-comment      [1mTrue[0m       Report as GitHub PR comment                
        --gh-summary      [1mTrue[0m       Report as GitHub step summary              
```
