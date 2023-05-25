(function () {
    "use strict";

    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    const on = (type, el, listener, all = false) => {
        if (all) {
            select(el, all).forEach(e => e.addEventListener(type, listener))
        } else {
            select(el, all).addEventListener(type, listener)
        }
    }

    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    if (select('.toggle-sidebar-btn')) {
        on('click', '.toggle-sidebar-btn', function (e) {
            select('body').classList.toggle('toggle-sidebar')
        })
    }

    if (select('.search-bar-toggle')) {
        on('click', '.search-bar-toggle', function (e) {
            select('.search-bar').classList.toggle('search-bar-show')
        })
    }

    let navbarlinks = select('#navbar .scrollto', true)
    const navbarlinksActive = () => {
        let position = window.scrollY + 200
        navbarlinks.forEach(navbarlink => {
            if (!navbarlink.hash) return
            let section = select(navbarlink.hash)
            if (!section) return
            if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
                navbarlink.classList.add('active')
            } else {
                navbarlink.classList.remove('active')
            }
        })
    }
    window.addEventListener('load', navbarlinksActive)
    onscroll(document, navbarlinksActive)

    let selectHeader = select('#header')
    if (selectHeader) {
        const headerScrolled = () => {
            if (window.scrollY > 100) {
                selectHeader.classList.add('header-scrolled')
            } else {
                selectHeader.classList.remove('header-scrolled')
            }
        }
        window.addEventListener('load', headerScrolled)
        onscroll(document, headerScrolled)
    }

    let backtotop = select('.back-to-top')
    if (backtotop) {
        const toggleBacktotop = () => {
            if (window.scrollY > 100) {
                backtotop.classList.add('active')
            } else {
                backtotop.classList.remove('active')
            }
        }
        window.addEventListener('load', toggleBacktotop)
        onscroll(document, toggleBacktotop)
    }

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })

    if (select('.quill-editor-default')) {
        new Quill('.quill-editor-default', {
            theme: 'snow'
        });
    }

    if (select('.quill-editor-bubble')) {
        new Quill('.quill-editor-bubble', {
            theme: 'bubble'
        });
    }

    if (select('.quill-editor-full')) {
        new Quill(".quill-editor-full", {
            modules: {
                toolbar: [
                    [{
                        font: []
                    }, {
                        size: []
                    }],
                    ["bold", "italic", "underline", "strike"],
                    [{
                        color: []
                    },
                    {
                        background: []
                    }
                    ],
                    [{
                        script: "super"
                    },
                    {
                        script: "sub"
                    }
                    ],
                    [{
                        list: "ordered"
                    },
                    {
                        list: "bullet"
                    },
                    {
                        indent: "-1"
                    },
                    {
                        indent: "+1"
                    }
                    ],
                    ["direction", {
                        align: []
                    }],
                    ["link", "image", "video"],
                    ["clean"]
                ]
            },
            theme: "snow"
        });
    }

    const useDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const isSmallScreen = window.matchMedia('(max-width: 1023.5px)').matches;

    tinymce.init({
        selector: 'textarea.tinymce-editor',
        plugins: 'preview importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap pagebreak nonbreaking anchor insertdatetime advlist lists wordcount help charmap quickbars emoticons',
        editimage_cors_hosts: ['picsum.photos'],
        menubar: 'file edit view insert format tools table help',
        toolbar: 'undo redo | bold italic underline strikethrough | fontfamily fontsize blocks | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',
        toolbar_sticky: true,
        toolbar_sticky_offset: isSmallScreen ? 102 : 108,
        autosave_ask_before_unload: true,
        autosave_interval: '30s',
        autosave_prefix: '{path}{query}-{id}-',
        autosave_restore_when_empty: false,
        autosave_retention: '2m',
        image_advtab: true,
        link_list: [{
            title: 'My page 1',
            value: 'https://www.tiny.cloud'
        },
        {
            title: 'My page 2',
            value: 'http://www.moxiecode.com'
        }
        ],
        image_list: [{
            title: 'My page 1',
            value: 'https://www.tiny.cloud'
        },
        {
            title: 'My page 2',
            value: 'http://www.moxiecode.com'
        }
        ],
        image_class_list: [{
            title: 'None',
            value: ''
        },
        {
            title: 'Some class',
            value: 'class-name'
        }
        ],
        importcss_append: true,
        file_picker_callback: (callback, value, meta) => {
            if (meta.filetype === 'file') {
                callback('https://www.google.com/logos/google.jpg', {
                    text: 'My text'
                });
            }

            if (meta.filetype === 'image') {
                callback('https://www.google.com/logos/google.jpg', {
                    alt: 'My alt text'
                });
            }

            if (meta.filetype === 'media') {
                callback('movie.mp4', {
                    source2: 'alt.ogg',
                    poster: 'https://www.google.com/logos/google.jpg'
                });
            }
        },
        templates: [{
            title: 'New Table',
            description: 'creates a new table',
            content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>'
        },
        {
            title: 'Starting my story',
            description: 'A cure for writers block',
            content: 'Once upon a time...'
        },
        {
            title: 'New list with dates',
            description: 'New List with dates',
            content: '<div class="mceTmpl"><span class="cdate">cdate</span><br><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>'
        }
        ],
        template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
        template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
        height: 600,
        image_caption: true,
        quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
        noneditable_class: 'mceNonEditable',
        toolbar_mode: 'sliding',
        contextmenu: 'link image table',
        skin: useDarkMode ? 'oxide-dark' : 'oxide',
        content_css: useDarkMode ? 'dark' : 'default',
        content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }'
    });

    var needsValidation = document.querySelectorAll('.needs-validation')

    Array.prototype.slice.call(needsValidation)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })

    const datatables = select('.datatable', true)
    datatables.forEach(datatable => {
        new simpleDatatables.DataTable(datatable);
    })

    const mainContainer = select('#main');
    if (mainContainer) {
        setTimeout(() => {
            new ResizeObserver(function () {
                select('.echart', true).forEach(getEchart => {
                    echarts.getInstanceByDom(getEchart).resize();
                })
            }).observe(mainContainer);
        }, 200);
    }

})();

function CreatePieChart(_id, _url) {
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            echarts.init(document.querySelector("#" + _id)).setOption({
                tooltip: {
                    trigger: 'item'
                },
                legend: {
                    top: '5%',
                    left: 'center'
                },
                series: [{
                    name: 'Access From',
                    type: 'pie',
                    radius: ['40%', '70%'],
                    avoidLabelOverlap: false,
                    label: {
                        show: false,
                        position: 'center'
                    },
                    emphasis: {
                        label: {
                            show: true,
                            fontSize: '18',
                            fontWeight: 'bold'
                        }
                    },
                    labelLine: {
                        show: false
                    },
                    data: _data
                }]
            });
        },
        error: function (xhr, errorType, exception) {
            console.error("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}

function CreateReportChart(_id) {
    new ApexCharts(document.querySelector("#" + _id), {
        series: [{
            name: 'A Book',
            data: [31, 40, 28, 51, 42, 82, 56],
        }, {
            name: 'B Book',
            data: [11, 32, 45, 32, 34, 52, 41]
        }, {
            name: 'C Book',
            data: [15, 11, 32, 18, 9, 24, 11]
        }],
        chart: {
            height: 350,
            type: 'area',
            toolbar: {
                show: false
            },
        },
        markers: {
            size: 4
        },
        colors: ['#4154f1', '#2eca6a', '#ff771d'],
        fill: {
            type: "gradient",
            gradient: {
                shadeIntensity: 1,
                opacityFrom: 0.3,
                opacityTo: 0.4,
                stops: [0, 90, 100]
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        xaxis: {
            type: 'datetime',
            categories: ["2018-09-19T00:00:00.000Z", "2018-09-19T01:30:00.000Z", "2018-09-19T02:30:00.000Z", "2018-09-19T03:30:00.000Z", "2018-09-19T04:30:00.000Z", "2018-09-19T05:30:00.000Z", "2018-09-19T06:30:00.000Z"]
        },
        tooltip: {
            x: {
                format: 'dd/MM/yy HH:mm'
            },
        }
    }).render();
}








function UpdateRecentReservations(_url) {
    $.ajax({
        type: 'GET',
        url: _url,
        cache: false,
        dataType: 'json',
        success: function (data) {
            console.log(data);
            $(".datatable tbody").empty();
            data.forEach(function (row) {
                let statusBadge;
                switch (row.status) {
                    case 1:
                        statusBadge = '<span class="badge bg-warning">Pending</span>';
                        break;
                    case 2:
                        statusBadge = '<span class="badge bg-success">Approved</span>';
                        break;
                    case 3:
                        statusBadge = '<span class="badge bg-danger">Rejected</span>';
                        break;
                    case 4:
                        statusBadge = '<span class="badge bg-warning">Not Enough Quota</span>';
                        break;
                    case 5:
                        statusBadge = '<span class="badge bg-dark">Past Time</span>';
                        break;
                    case 6:
                        statusBadge = '<span class="badge bg-warning">Deleted</span>';
                        break;
                }

                let newRow = '<tr>' +
                    '<th scope="row"><a href="#">#' + row.id + '</a></th>' +
                    '<td>' + row.userName + '</td>' +
                    '<td><a href="#" class="text-primary">' + row.roomName + '</a></td>' +
                    '<td>' + row.createdTime+ '</td>' +
                    '<td>' + statusBadge + '</td>' +
                    '</tr>';
                $(".datatable tbody").append(newRow);
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}
=======
(function () {
    "use strict";

    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    const on = (type, el, listener, all = false) => {
        if (all) {
            select(el, all).forEach(e => e.addEventListener(type, listener))
        } else {
            select(el, all).addEventListener(type, listener)
        }
    }

    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    if (select('.toggle-sidebar-btn')) {
        on('click', '.toggle-sidebar-btn', function (e) {
            select('body').classList.toggle('toggle-sidebar')
        })
    }

    if (select('.search-bar-toggle')) {
        on('click', '.search-bar-toggle', function (e) {
            select('.search-bar').classList.toggle('search-bar-show')
        })
    }

    let navbarlinks = select('#navbar .scrollto', true)
    const navbarlinksActive = () => {
        let position = window.scrollY + 200
        navbarlinks.forEach(navbarlink => {
            if (!navbarlink.hash) return
            let section = select(navbarlink.hash)
            if (!section) return
            if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
                navbarlink.classList.add('active')
            } else {
                navbarlink.classList.remove('active')
            }
        })
    }
    window.addEventListener('load', navbarlinksActive)
    onscroll(document, navbarlinksActive)

    let selectHeader = select('#header')
    if (selectHeader) {
        const headerScrolled = () => {
            if (window.scrollY > 100) {
                selectHeader.classList.add('header-scrolled')
            } else {
                selectHeader.classList.remove('header-scrolled')
            }
        }
        window.addEventListener('load', headerScrolled)
        onscroll(document, headerScrolled)
    }

    let backtotop = select('.back-to-top')
    if (backtotop) {
        const toggleBacktotop = () => {
            if (window.scrollY > 100) {
                backtotop.classList.add('active')
            } else {
                backtotop.classList.remove('active')
            }
        }
        window.addEventListener('load', toggleBacktotop)
        onscroll(document, toggleBacktotop)
    }

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })

    if (select('.quill-editor-default')) {
        new Quill('.quill-editor-default', {
            theme: 'snow'
        });
    }

    if (select('.quill-editor-bubble')) {
        new Quill('.quill-editor-bubble', {
            theme: 'bubble'
        });
    }

    if (select('.quill-editor-full')) {
        new Quill(".quill-editor-full", {
            modules: {
                toolbar: [
                    [{
                        font: []
                    }, {
                        size: []
                    }],
                    ["bold", "italic", "underline", "strike"],
                    [{
                        color: []
                    },
                    {
                        background: []
                    }
                    ],
                    [{
                        script: "super"
                    },
                    {
                        script: "sub"
                    }
                    ],
                    [{
                        list: "ordered"
                    },
                    {
                        list: "bullet"
                    },
                    {
                        indent: "-1"
                    },
                    {
                        indent: "+1"
                    }
                    ],
                    ["direction", {
                        align: []
                    }],
                    ["link", "image", "video"],
                    ["clean"]
                ]
            },
            theme: "snow"
        });
    }

    const useDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const isSmallScreen = window.matchMedia('(max-width: 1023.5px)').matches;

    tinymce.init({
        selector: 'textarea.tinymce-editor',
        plugins: 'preview importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap pagebreak nonbreaking anchor insertdatetime advlist lists wordcount help charmap quickbars emoticons',
        editimage_cors_hosts: ['picsum.photos'],
        menubar: 'file edit view insert format tools table help',
        toolbar: 'undo redo | bold italic underline strikethrough | fontfamily fontsize blocks | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',
        toolbar_sticky: true,
        toolbar_sticky_offset: isSmallScreen ? 102 : 108,
        autosave_ask_before_unload: true,
        autosave_interval: '30s',
        autosave_prefix: '{path}{query}-{id}-',
        autosave_restore_when_empty: false,
        autosave_retention: '2m',
        image_advtab: true,
        link_list: [{
            title: 'My page 1',
            value: 'https://www.tiny.cloud'
        },
        {
            title: 'My page 2',
            value: 'http://www.moxiecode.com'
        }
        ],
        image_list: [{
            title: 'My page 1',
            value: 'https://www.tiny.cloud'
        },
        {
            title: 'My page 2',
            value: 'http://www.moxiecode.com'
        }
        ],
        image_class_list: [{
            title: 'None',
            value: ''
        },
        {
            title: 'Some class',
            value: 'class-name'
        }
        ],
        importcss_append: true,
        file_picker_callback: (callback, value, meta) => {
            if (meta.filetype === 'file') {
                callback('https://www.google.com/logos/google.jpg', {
                    text: 'My text'
                });
            }

            if (meta.filetype === 'image') {
                callback('https://www.google.com/logos/google.jpg', {
                    alt: 'My alt text'
                });
            }

            if (meta.filetype === 'media') {
                callback('movie.mp4', {
                    source2: 'alt.ogg',
                    poster: 'https://www.google.com/logos/google.jpg'
                });
            }
        },
        templates: [{
            title: 'New Table',
            description: 'creates a new table',
            content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>'
        },
        {
            title: 'Starting my story',
            description: 'A cure for writers block',
            content: 'Once upon a time...'
        },
        {
            title: 'New list with dates',
            description: 'New List with dates',
            content: '<div class="mceTmpl"><span class="cdate">cdate</span><br><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>'
        }
        ],
        template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
        template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
        height: 600,
        image_caption: true,
        quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
        noneditable_class: 'mceNonEditable',
        toolbar_mode: 'sliding',
        contextmenu: 'link image table',
        skin: useDarkMode ? 'oxide-dark' : 'oxide',
        content_css: useDarkMode ? 'dark' : 'default',
        content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }'
    });

    var needsValidation = document.querySelectorAll('.needs-validation')

    Array.prototype.slice.call(needsValidation)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })

    const datatables = select('.datatable', true)
    datatables.forEach(datatable => {
        new simpleDatatables.DataTable(datatable);
    })

    const mainContainer = select('#main');
    if (mainContainer) {
        setTimeout(() => {
            new ResizeObserver(function () {
                select('.echart', true).forEach(getEchart => {
                    echarts.getInstanceByDom(getEchart).resize();
                })
            }).observe(mainContainer);
        }, 200);
    }

})();

function CreatePieChart(_id, _url) {
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            echarts.init(document.querySelector("#" + _id)).setOption({
                tooltip: {
                    trigger: 'item'
                },
                legend: {
                    top: '5%',
                    left: 'center'
                },
                series: [{
                    name: 'Access From',
                    type: 'pie',
                    radius: ['40%', '70%'],
                    avoidLabelOverlap: false,
                    label: {
                        show: false,
                        position: 'center'
                    },
                    emphasis: {
                        label: {
                            show: true,
                            fontSize: '18',
                            fontWeight: 'bold'
                        }
                    },
                    labelLine: {
                        show: false
                    },
                    data: _data
                }]
            });
        },
        error: function (xhr, errorType, exception) {
            console.error("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}

function CreateReportChart(_id) {
    new ApexCharts(document.querySelector("#" + _id), {
        series: [{
            name: 'A Book',
            data: [31, 40, 28, 51, 42, 82, 56],
        }, {
            name: 'B Book',
            data: [11, 32, 45, 32, 34, 52, 41]
        }, {
            name: 'C Book',
            data: [15, 11, 32, 18, 9, 24, 11]
        }],
        chart: {
            height: 350,
            type: 'area',
            toolbar: {
                show: false
            },
        },
        markers: {
            size: 4
        },
        colors: ['#4154f1', '#2eca6a', '#ff771d'],
        fill: {
            type: "gradient",
            gradient: {
                shadeIntensity: 1,
                opacityFrom: 0.3,
                opacityTo: 0.4,
                stops: [0, 90, 100]
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        xaxis: {
            type: 'datetime',
            categories: ["2018-09-19T00:00:00.000Z", "2018-09-19T01:30:00.000Z", "2018-09-19T02:30:00.000Z", "2018-09-19T03:30:00.000Z", "2018-09-19T04:30:00.000Z", "2018-09-19T05:30:00.000Z", "2018-09-19T06:30:00.000Z"]
        },
        tooltip: {
            x: {
                format: 'dd/MM/yy HH:mm'
            },
        }
    }).render();
}

function findBook(_id, _url) {
    var selectedCheckboxes = document.querySelectorAll("input[type=checkbox]:checked");
    // Create an array to store the selected checkbox values
    var selectedValues = {
        "content": [],
        "subject": [],
        "author": [],
        "language": []
    };
    // Iterate over the selected checkboxes
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        // Add the value of each selected checkbox to the array
        var identifier = selectedCheckboxes[i].id.split("=")[0];
        value = selectedCheckboxes[i].id.split("=")[1];

        if (identifier === "contentType") {
            selectedValues["content"].push(value);
        }
        else if (identifier === "subjectTerm") {
            selectedValues["subject"].push(value);
        }
        else if (identifier === "author") {
            selectedValues["author"].push(value);
        }
        else if (identifier === "language") {
            selectedValues["language"].push(value);
        }
    }

    var searchInput = document.querySelector("#search-input").value.trim();
    var startDateValue = document.querySelector("#start-date").value;
    var endDateValue = document.querySelector("#end-date").value;

    var requestData = {
        keyword: searchInput,
        startDate: startDateValue,
        endDate: endDateValue,
        content: selectedValues["content"],
        subject: selectedValues["subject"],
        author: selectedValues["author"],
        language: selectedValues["language"]
    };

    console.log(requestData);

    debugger;
    $.ajax({
        type: 'POST',
        url: _url,
        datatype: 'json',
        data: JSON.stringify(requestData),
        contentType: 'application/json; charset=UTF-8',
        cache: false,
        success: function (_data) {
            debugger;

            var listGroup = document.querySelector("#" + _id);
            listGroup.innerHTML = "";

            _data.forEach(function (item) {
                var listItem = document.createElement("li");
                listItem.classList.add("list-group-item");

                var title = document.createElement("h3");
                title.textContent = "Title: " + item.title;

                var author = document.createElement("p");
                author.textContent = "Authors: " + item.authors[0];

                var publicationDate = document.createElement("p");
                publicationDate.textContent = "Publication: " + item.publicationDate;

                var content = document.createElement("p");
                content.textContent = "Contents: " + item.contents[0];


                listItem.appendChild(title);
                listItem.appendChild(author);
                listItem.appendChild(publicationDate);
                listItem.appendChild(content);

                listGroup.appendChild(listItem);

            });
        },
        error: function (xhr, errorType, exception) {
            console.log("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}

function getContents(_id, _url) {
    debugger;
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            debugger;
            var listGroup = document.querySelector("#" + _id);
            listGroup.innerHTML = "";

            i = 0;
            _data.forEach(function (item) {

                inputItem = document.createElement("input");
                inputItem.classList.add("form-check-input");
                inputItem.setAttribute("type", "checkbox");
                inputItem.setAttribute("id", "contentType=" + item.id);

                labelItem = document.createElement("label");
                labelItem.textContent = item.name;
                labelItem.classList.add("form-check-label")
                labelItem.setAttribute("for", "contentType=" + item.id);

                brItem = document.createElement("br");

                i++;

                listGroup.appendChild(inputItem);
                listGroup.appendChild(labelItem);
                listGroup.appendChild(brItem);

            });
        },
        error: function (xhr, errorType, exception) {
            console.log("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}

function getSubjects(_id, _url) {
    debugger;
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            debugger;
            var listGroup = document.querySelector("#" + _id);
            listGroup.innerHTML = "";

            i = 0;
            _data.forEach(function (item) {

                inputItem = document.createElement("input");
                inputItem.classList.add("form-check-input");
                inputItem.setAttribute("type", "checkbox");
                inputItem.setAttribute("id", "subjectTerm=" + item.id);

                labelItem = document.createElement("label");
                labelItem.textContent = item.name;
                labelItem.classList.add("form-check-label")
                labelItem.setAttribute("for", "subjectTerm=" + item.id);

                brItem = document.createElement("br");

                i++;

                listGroup.appendChild(inputItem);
                listGroup.appendChild(labelItem);
                listGroup.appendChild(brItem);

            });
        },
        error: function (xhr, errorType, exception) {
            console.log("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}


function getLanguages(_id, _url) {
    debugger;
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            debugger;
            var listGroup = document.querySelector("#" + _id);
            listGroup.innerHTML = "";

            i = 0;
            _data.forEach(function (item) {

                inputItem = document.createElement("input");
                inputItem.classList.add("form-check-input");
                inputItem.setAttribute("type", "checkbox");
                inputItem.setAttribute("id", "language=" + item.id);

                labelItem = document.createElement("label");
                labelItem.textContent = item.name;
                labelItem.classList.add("form-check-label")
                labelItem.setAttribute("for", "language=" + item.id);

                brItem = document.createElement("br");

                i++;

                listGroup.appendChild(inputItem);
                listGroup.appendChild(labelItem);
                listGroup.appendChild(brItem);

            });
        },
        error: function (xhr, errorType, exception) {
            console.log("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}


function getAuthors(_id, _url) {
    debugger;
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            debugger;
            var listGroup = document.querySelector("#" + _id);
            listGroup.innerHTML = "";

            i = 0;
            _data.forEach(function (item) {

                inputItem = document.createElement("input");
                inputItem.classList.add("form-check-input");
                inputItem.setAttribute("type", "checkbox");
                inputItem.setAttribute("id", "author=" + item.id);

                labelItem = document.createElement("label");
                labelItem.textContent = item.name + " " + item.surname;
                labelItem.classList.add("form-check-label")
                labelItem.setAttribute("for", "author=" + item.id);

                brItem = document.createElement("br");

                i++;

                listGroup.appendChild(inputItem);
                listGroup.appendChild(labelItem);
                listGroup.appendChild(brItem);

            });
            listGroup.classList.add("vertical-scrollable");
        },
        error: function (xhr, errorType, exception) {
            console.log("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}

function getInitialBooks(_id, _url) {

    debugger;
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            debugger;

            var listGroup = document.querySelector("#" + _id);
            listGroup.innerHTML = "";

            _data.forEach(function (item) {
                var listItem = document.createElement("li");
                listItem.classList.add("list-group-item");

                var title = document.createElement("h3");
                title.textContent = "Title: " + item.title;

                var author = document.createElement("p");
                author.textContent = "Authors: " + item.authors[0];

                var publicationDate = document.createElement("p");
                publicationDate.textContent = "Publication: " + item.publicationDate;

                var content = document.createElement("p");
                content.textContent = "Contents: " + item.contents[0];


                listItem.appendChild(title);
                listItem.appendChild(author);
                listItem.appendChild(publicationDate);
                listItem.appendChild(content);

                listGroup.appendChild(listItem);

            });
        },
        error: function (xhr, errorType, exception) {
            console.log("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}

=======
﻿(function () {
    "use strict";

    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    const on = (type, el, listener, all = false) => {
        if (all) {
            select(el, all).forEach(e => e.addEventListener(type, listener))
        } else {
            select(el, all).addEventListener(type, listener)
        }
    }

    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    if (select('.toggle-sidebar-btn')) {
        on('click', '.toggle-sidebar-btn', function (e) {
            select('body').classList.toggle('toggle-sidebar')
        })
    }

    if (select('.search-bar-toggle')) {
        on('click', '.search-bar-toggle', function (e) {
            select('.search-bar').classList.toggle('search-bar-show')
        })
    }

    let navbarlinks = select('#navbar .scrollto', true)
    const navbarlinksActive = () => {
        let position = window.scrollY + 200
        navbarlinks.forEach(navbarlink => {
            if (!navbarlink.hash) return
            let section = select(navbarlink.hash)
            if (!section) return
            if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
                navbarlink.classList.add('active')
            } else {
                navbarlink.classList.remove('active')
            }
        })
    }
    window.addEventListener('load', navbarlinksActive)
    onscroll(document, navbarlinksActive)

    let selectHeader = select('#header')
    if (selectHeader) {
        const headerScrolled = () => {
            if (window.scrollY > 100) {
                selectHeader.classList.add('header-scrolled')
            } else {
                selectHeader.classList.remove('header-scrolled')
            }
        }
        window.addEventListener('load', headerScrolled)
        onscroll(document, headerScrolled)
    }

    let backtotop = select('.back-to-top')
    if (backtotop) {
        const toggleBacktotop = () => {
            if (window.scrollY > 100) {
                backtotop.classList.add('active')
            } else {
                backtotop.classList.remove('active')
            }
        }
        window.addEventListener('load', toggleBacktotop)
        onscroll(document, toggleBacktotop)
    }

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })

    if (select('.quill-editor-default')) {
        new Quill('.quill-editor-default', {
            theme: 'snow'
        });
    }

    if (select('.quill-editor-bubble')) {
        new Quill('.quill-editor-bubble', {
            theme: 'bubble'
        });
    }

    if (select('.quill-editor-full')) {
        new Quill(".quill-editor-full", {
            modules: {
                toolbar: [
                    [{
                        font: []
                    }, {
                        size: []
                    }],
                    ["bold", "italic", "underline", "strike"],
                    [{
                        color: []
                    },
                    {
                        background: []
                    }
                    ],
                    [{
                        script: "super"
                    },
                    {
                        script: "sub"
                    }
                    ],
                    [{
                        list: "ordered"
                    },
                    {
                        list: "bullet"
                    },
                    {
                        indent: "-1"
                    },
                    {
                        indent: "+1"
                    }
                    ],
                    ["direction", {
                        align: []
                    }],
                    ["link", "image", "video"],
                    ["clean"]
                ]
            },
            theme: "snow"
        });
    }

    const useDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const isSmallScreen = window.matchMedia('(max-width: 1023.5px)').matches;

    tinymce.init({
        selector: 'textarea.tinymce-editor',
        plugins: 'preview importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap pagebreak nonbreaking anchor insertdatetime advlist lists wordcount help charmap quickbars emoticons',
        editimage_cors_hosts: ['picsum.photos'],
        menubar: 'file edit view insert format tools table help',
        toolbar: 'undo redo | bold italic underline strikethrough | fontfamily fontsize blocks | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',
        toolbar_sticky: true,
        toolbar_sticky_offset: isSmallScreen ? 102 : 108,
        autosave_ask_before_unload: true,
        autosave_interval: '30s',
        autosave_prefix: '{path}{query}-{id}-',
        autosave_restore_when_empty: false,
        autosave_retention: '2m',
        image_advtab: true,
        link_list: [{
            title: 'My page 1',
            value: 'https://www.tiny.cloud'
        },
        {
            title: 'My page 2',
            value: 'http://www.moxiecode.com'
        }
        ],
        image_list: [{
            title: 'My page 1',
            value: 'https://www.tiny.cloud'
        },
        {
            title: 'My page 2',
            value: 'http://www.moxiecode.com'
        }
        ],
        image_class_list: [{
            title: 'None',
            value: ''
        },
        {
            title: 'Some class',
            value: 'class-name'
        }
        ],
        importcss_append: true,
        file_picker_callback: (callback, value, meta) => {
            if (meta.filetype === 'file') {
                callback('https://www.google.com/logos/google.jpg', {
                    text: 'My text'
                });
            }

            if (meta.filetype === 'image') {
                callback('https://www.google.com/logos/google.jpg', {
                    alt: 'My alt text'
                });
            }

            if (meta.filetype === 'media') {
                callback('movie.mp4', {
                    source2: 'alt.ogg',
                    poster: 'https://www.google.com/logos/google.jpg'
                });
            }
        },
        templates: [{
            title: 'New Table',
            description: 'creates a new table',
            content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>'
        },
        {
            title: 'Starting my story',
            description: 'A cure for writers block',
            content: 'Once upon a time...'
        },
        {
            title: 'New list with dates',
            description: 'New List with dates',
            content: '<div class="mceTmpl"><span class="cdate">cdate</span><br><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>'
        }
        ],
        template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
        template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
        height: 600,
        image_caption: true,
        quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
        noneditable_class: 'mceNonEditable',
        toolbar_mode: 'sliding',
        contextmenu: 'link image table',
        skin: useDarkMode ? 'oxide-dark' : 'oxide',
        content_css: useDarkMode ? 'dark' : 'default',
        content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }'
    });

    var needsValidation = document.querySelectorAll('.needs-validation')

    Array.prototype.slice.call(needsValidation)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })

    const datatables = select('.datatable', true)
    datatables.forEach(datatable => {
        new simpleDatatables.DataTable(datatable);
    })

    const mainContainer = select('#main');
    if (mainContainer) {
        setTimeout(() => {
            new ResizeObserver(function () {
                select('.echart', true).forEach(getEchart => {
                    echarts.getInstanceByDom(getEchart).resize();
                })
            }).observe(mainContainer);
        }, 200);
    }

})();

function CreatePieChart(_id, _url) {
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            echarts.init(document.querySelector("#" + _id)).setOption({
                tooltip: {
                    trigger: 'item'
                },
                legend: {
                    top: '5%',
                    left: 'center'
                },
                series: [{
                    name: 'Access From',
                    type: 'pie',
                    radius: ['40%', '70%'],
                    avoidLabelOverlap: false,
                    label: {
                        show: false,
                        position: 'center'
                    },
                    emphasis: {
                        label: {
                            show: true,
                            fontSize: '18',
                            fontWeight: 'bold'
                        }
                    },
                    labelLine: {
                        show: false
                    },
                    data: _data
                }]
            });
        },
        error: function (xhr, errorType, exception) {
            console.error("error: ", xhr, " ", errorType, " ", exception);
        }
    });
}

function CreateReportChart(_id) {
    new ApexCharts(document.querySelector("#" + _id), {
        series: [{
            name: 'A Book',
            data: [31, 40, 28, 51, 42, 82, 56],
        }, {
            name: 'B Book',
            data: [11, 32, 45, 32, 34, 52, 41]
        }, {
            name: 'C Book',
            data: [15, 11, 32, 18, 9, 24, 11]
        }],
        chart: {
            height: 350,
            type: 'area',
            toolbar: {
                show: false
            },
        },
        markers: {
            size: 4
        },
        colors: ['#4154f1', '#2eca6a', '#ff771d'],
        fill: {
            type: "gradient",
            gradient: {
                shadeIntensity: 1,
                opacityFrom: 0.3,
                opacityTo: 0.4,
                stops: [0, 90, 100]
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        xaxis: {
            type: 'datetime',
            categories: ["2018-09-19T00:00:00.000Z", "2018-09-19T01:30:00.000Z", "2018-09-19T02:30:00.000Z", "2018-09-19T03:30:00.000Z", "2018-09-19T04:30:00.000Z", "2018-09-19T05:30:00.000Z", "2018-09-19T06:30:00.000Z"]
        },
        tooltip: {
            x: {
                format: 'dd/MM/yy HH:mm'
            },
        }
    }).render();
}

function UpdateRoomResStats(resCountUrl, resRateUrl) {
    $.ajax({
        type: 'GET',
        url: resCountUrl,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            $("#reservation-count").text(_data);
function UpdateUserStatistics(userCountUrl, userIncreaseRateUrl) {
    $.ajax({
        type: 'GET',
        url: userCountUrl,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            $("#total-users").text(_data);
        },
        error: function (xhr, errorType, exception) {
            console.log("error: ", xhr, " ", errorType, " ", exception);
        }
    });

    $.ajax({
        type: 'GET',
        url: resRateUrl,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            var rate = (Math.abs(_data) * 100).toFixed(2);
            var label = _data > 0 ? "increase" : "decrease";
            $("#reservation-rate").text(rate + "%");
            $("#reservation-rate").addClass(_data > 0 ? "text-success" : "text-danger");
            $("#reservation-label").text(label);
            $("#user-increase-rate").text(rate + "%");
            $("#user-increase-rate").addClass(_data > 0 ? "text-success" : "text-danger");
            $("#increase-decrease-label").text(label);

  
function UpdateBestRankedBook(_id, _url) {
    $.ajax({
        type: 'GET',
        url: _url,
        datatype: 'json',
        cache: false,
        success: function (_data) {
            var activityDiv = $("#" + _id).find(".activity");
            activityDiv.empty();

            _data.forEach(function (book) {
                var date = moment(book.addedDate);
                var now = moment();
                var addTimeAgo = moment.duration(now.diff(date)).humanize() + ' ago';

                var item = '<div class="activity-item d-flex">' +
                    '<div class="activite-label">' + addTimeAgo + '</div>' +
                    '<i class="bi bi-circle-fill activity-badge text-success align-self-start"></i>' +
                    '<div class="activity-content">' + book.title +
                    '</div></div>';
                activityDiv.append(item);
            });
            var bookElement = document.querySelector('#' + _id + ' h6');
            var ratingElement = document.querySelector('#' + _id + ' span');

            bookElement.innerHTML = _data.title;
            ratingElement.innerHTML = "Rating : " + _data.rating;
        },
        error: function (xhr, errorType, exception) {
            console.log("error: ", xhr, " ", errorType, " ", exception);
        }
    });
