function CreateScheduler(_id, _startDate, _listUrl, _addUrl, _updateUrl, _deleteUrl) {
    const dp = new DayPilot.Scheduler(_id, {
        startDate: _startDate,
        days: 365,
        scale: "Hour",
        timeHeaders: [
            { groupBy: "Month", format: "MMMM yyyy" },
            { groupBy: "Day", format: "d" },
            { groupBy: "Hour", format: "h"}
        ],
        treeEnabled: true,
        treePreventParentUsage: true,
        heightSpec: "Max",
        height: 400,
        eventMovingStartEndEnabled: true,
        eventResizingStartEndEnabled: true,
        timeRangeSelectingStartEndEnabled: true,
        contextMenu: new DayPilot.Menu({
            items: [
                {
                    text: "Edit",
                    onClick: (args) => {
                        console.log(args);
                        dp.events.edit(args.source);
                    }
                },
                {
                    text: "Delete",
                    onClick: (args) => {
                        console.log(args);
                        dp.events.remove(args.source);
                    }
                },
                { text: "-" },
                {
                    text: "Select",
                    onClick: (args) => {
                        console.log(args);
                        dp.multiselect.add(args.source);
                    }
                }
            ]
        }),
        onEventMoved: (args) => {
            console.log(args);
            dp.message("Moved: " + args.e.text());
        },
        onEventMoving: (args) => {
            // TODO: Update
            args.allowed = false;
        },
        onEventResized: (args) => {
            // TODO: Update
            console.log(args);
            dp.message("Resized: " + args.e.text());
        },
        onTimeRangeSelected: async (args) => {
            const modal = await DayPilot.Modal.prompt("New reservation: ", "Your email address")
            dp.clearSelection();
            if (modal.canceled) {
                return;
            }
            console.log(args);
            const postData = {
                start: args.start.value,
                end: args.end.value,
                resource: args.resource,
                email: modal.result
            };
            console.log(postData);

            $.ajax({
                type: 'POST',
                url: _addUrl,
                datatype: 'json',
                cache: false,
                data: JSON.stringify(postData),
                contentType: 'application/json; charset=UTF-8',
                success: function (_data) {
                    if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                        // dp.events.add({
                        //     start: args.start,
                        //     end: args.end,
                        //     id: DayPilot.guid(),
                        //     resource: args.resource,
                        //     text: name
                        // });
                        dp.message("Created");
                    }
                    else {
                        console.error("error: ", _data.ExceptionMessage);
                    }
                },
                error: function (xhr, errorType, exception) {
                    console.error("error: ", xhr, " ", errorType, " ", exception);
                }
            });
        },
        onEventMove: (args) => {
            // TODO: Update
            console.log(args);
            if (args.ctrl) {
                dp.events.add({
                    start: args.newStart,
                    end: args.newEnd,
                    text: "Copy of " + args.e.text(),
                    resource: args.newResource,
                    id: DayPilot.guid()
                });

                args.preventDefault();
            }
        },
        onEventClick: (args) => {
            DayPilot.Modal.alert(args.e.data.text);
        }
    });

    dp.init();
    dp.scrollTo("2023-03-01");

    const app = {
        barColor(i) {
            const colors = ["#3c78d8", "#6aa84f", "#f1c232", "#cc0000"];
            return colors[i % 4];
        },
        barBackColor(i) {
            const colors = ["#a4c2f4", "#b6d7a8", "#ffe599", "#ea9999"];
            return colors[i % 4];
        },
        loadData() {
            $.ajax({
                type: 'GET',
                url: _listUrl,
                datatype: 'json',
                cache: false,
                success: function (_data) {
                    if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                        const resources = _data.data.resources;
                        const events = [];
                        for (let i = 0; i < _data.data.events.length; i++) {
                            events.push({
                                start: new DayPilot.Date(_data.data.events[i].start),
                                end: new DayPilot.Date(_data.data.events[i].end),
                                id: _data.data.events[i].id,
                                resource: _data.data.events[i].resource,
                                text: _data.data.events[i].text,
                                bubbleHtml: _data.data.events[i].bubbleHtml,
                                barColor: app.barColor(i),
                                barBackColor: app.barBackColor(i)
                            });
                        }

                        dp.update({ resources, events });
                    }
                    else {
                        console.error("error: ", _data.ExceptionMessage);
                    }
                },
                error: function (xhr, errorType, exception) {
                    console.error("error: ", xhr, " ", errorType, " ", exception);
                }
            });
        },
    };

    app.loadData();
}
