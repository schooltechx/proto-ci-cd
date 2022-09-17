# proto-ci-cd


# Git 
    git config --global user.name "oom"
    git config --global user.email "schooltechx@gmail.com"
    git config --list
    # สร้าง .github/workflows

    # ค้นหา github action marketplace

# Act
    # https://juliusgamanyi.com/2022/04/27/run-github-actions-workflows-locally-using-act/#Download_and_configure_act
    
    # ติดตั้ง nektos/actact https://github.com/nektos/act/releases
    # เลือกรันคำสั่งล่างนี้ Medium มันจะสร้าง ~/.actrc ให้
    act --dryrun 
    act --dryrun --workflows .github/workflows/build-image.yaml --verbose --list
    # รันตาม job
    act --workflows .github/workflows/build-image.yaml --verbose --job build 
    # รันตาม event
    act --workflows .github/workflows/build-image.yaml push

# Docker
    # dev build and test
    docker build -t user-api .
    docker run -d -p 8083:80 --network proxy --name user-api user-api
    docker login docker.frappet.com
    docker images
    # keep in docker registry for testing/deploy with version tag
    docker image tag user-api:latest docker.frappet.com/demo/user-api:latest
    docker image tag user-api:latest docker.frappet.com/demo/user-api:0.7
    docker image push docker.frappet.com/demo/user-api:latest
    docker image push docker.frappet.com/demo/user-api:0.7