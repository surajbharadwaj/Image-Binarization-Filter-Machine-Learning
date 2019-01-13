# Image-Binarization-Filter-Machine-Learning
Implementation of Image Binarization techniques and enhancement techniques in C#. Medain filter,Convolution filter,Dynamic Gaussian,Gaussian filter


In this project an approach for image binarization has been presented. Given a piece of image of a fixed size, the model outputs a selectional value for each pixel of the image depending on the confidence whether the pixel belongs to the foreground of the document. These values are eventually thresholded to yield a discrete binary result.

Global thresholding method is better approach for calculate the threshold values of a grey scale images. But it doesn’t give good results for colored image and under intensity illumination. Therefore Otsu Threshold is an effective method to calculate the threshold of an image dynamically as Otsu is used to, automatically perform clustering-based image thresholding. 

In a world saturated with technology and information, digital signals are literally everywhere. Much of the technology that exists today would not be possible without a means of extracting information and manipulating digital signals. One such method for processing digital images, called filtering, can be used to reduce unwanted signal information (noise) or extract information such as edges in the image.                            The converted original image to grayscale contains a certain amount of noise in it. Depending upon the requirement we can use median filter, convolution filter or other filtering techniques. 
