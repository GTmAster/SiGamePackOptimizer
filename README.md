![unit tests shield](https://img.shields.io/github/workflow/status/GTmAster/SiGamePackOptimizer/Unit%20tests/master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SiGamePackOptimizer&metric=alert_status)](https://sonarcloud.io/dashboard?id=SiGamePackOptimizer)
# SiGamePackOptimizer
Optimizer for SiGame question packs

# Usage
```
SiGamePackOptimizer.exe <arguments>
  -z, --optimizers      Required. Optimizers to be used, comma separated. Available optimizers: ImageSizeReducer
  -i, --input           Required. Path to input file
  -o, --output          Required. Path to output file
  --help                Display this help screen.
  --version             Display version information.
```  
  
## ImageSizeReducer arguments
```
  --imagemaxwidth       (Default: 1024) Maximum width of image. All images large then provided width will be resized
                        preserving aspect ratio                     
  --imagemaxheight      (Default: 768) Maximum height of image. All images large then provided height will be resized
                        preserving aspect ratio                       
  --imagejpegquality    (Default: 75) Jpeg quality for images. From 0 to 100. Larger number - better quality and larger
                        size. All images will be encoded in JPEG format with given quality
```                        
## Example
Optimize pack using ImageSizeReducer: make all images 800x600 at most with default JPEG quality

`SiGamePackOptimizer.exe -z ImageSizeReducer -i C:\MyDocuments\pack.siq -o C:\MyDocuments\optimized.siq --imagemaxwidth 800 --imagemaxheight 600`

