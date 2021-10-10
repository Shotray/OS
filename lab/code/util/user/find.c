#include "kernel/types.h"
#include "kernel/stat.h"
#include "user/user.h"
#include "kernel/fs.h"

char* findFileName(char* path)
{
    char* p;
    // find first character after last '/'
    for(p=path+strlen(path);p>=path && *p!='/'; p--);
    p++;
    return p;
}


void find(char* path,char* target)
{
    char buf[512],*p;
    int fd;
    struct dirent de;
    struct stat st;

    if((fd=open(path,0))<0)
    {
       fprintf(2,"find: cannot open %s\n",path);
       return;
    }

    if(fstat(fd,&st)<0)
    {
       fprintf(2,"find: connot open %s\n",path);
       return;
    }

    switch(st.type)
    {
      case T_FILE:  {
	         if(strcmp(findFileName(path),target)==0) printf("%s\n",path);
		 break; 
       }
      case T_DIR: {
	 if(strlen(path)+1+DIRSIZ+1 >sizeof buf)
	 {
	   printf("find: path too long\n");
	   break;
	 }	
         strcpy(buf,path);
         p=buf+strlen(buf);
         *p++='/';
         while(read(fd, &de ,sizeof(de))==sizeof(de))
	 {
           if(de.inum == 0 || strcmp(de.name,".")==0 || strcmp(de.name,"..")==0)  continue;
	    memmove(p,de.name,DIRSIZ);
	   // printf("%s\n",de.name);
	    p[DIRSIZ]=0;
	    if(stat(buf,&st)<0)
	    {
	       printf("find: cannot stat %s\n",buf);
	       continue;
	    }
	    
           // if(strcmp(findFileName(buf),target)==0) printf("%s\n",buf);
	    find(buf,target);
	 }
        break;	 
       }		  
    }
    close(fd);
}

int main(int argc,char* argv[])
{
  if(argc<3 || argc>3)
  {
    printf("ONly three argement is needed!\n");	  
    exit(1);
  }
  find(argv[1],argv[2]);
  exit(0);
}

