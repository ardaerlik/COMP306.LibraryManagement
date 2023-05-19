﻿function CreateScheduler(_id, _startDate, _listUrl, _addUrl, _updateUrl, _deleteUrl, _email) {
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
                        const postData = {
                            start : args.newStart.value,
                            end : args.newEnd.value,
                            resource : args.newResource,
                            email : _email,
                            reservationId : args.e.data.id
                        };
            
                        $.ajax({
                            type: 'POST',
                            url: _updateUrl,
                            datatype: 'json',
                            cache: false,
                            data: JSON.stringify(postData),
                            contentType: 'application/json; charset=UTF-8',
                            success: function (_data) {
                                if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                                    var oldEvent = scheduler.events.find(function(e) {
                                        return e.id === args.e.data.id;
                                    });
            
                                    if (oldEvent) {
                                        dp.events.remove(oldEvent);
            
                                        dp.events.add({
                                            start: new DayPilot.Date(_data.data.start),
                                            end: new DayPilot.Date(_data.data.end),
                                            id: _data.data.id,
                                            resource: _data.data.resource,
                                            text: _data.data.text,
                                            bubbleHtml: _data.data.bubbleHtml,
                                        });
            
                                        dp.update();
                                        dp.message("Reservation is been updated for " + _data.data.start + " " + _data.data.end + ".");
                                    }
                                    else {
                                        dp.message("Reservation cannot been updated for " + _data.data.start + " " + _data.data.end + ".");
                                    }
                                }
                                else {
                                    dp.message(_data.ExceptionMessage);
                                    console.error("error: ", _data.ExceptionMessage);
                                }
                            },
                            error: function (xhr, errorType, exception) {
                                dp.message("error: " + xhr + " " + errorType + " " + exception);
                                console.error("error: ", xhr, " ", errorType, " ", exception);
                            }
                        });
                    }
                },
                {
                    text: "Delete",
                    onClick: (args) => {
                        const postData = {
                            email : _email,
                            reservationId : args.e.data.id
                        };
            
                        $.ajax({
                            type: 'POST',
                            url: _deleteUrl,
                            datatype: 'json',
                            cache: false,
                            data: JSON.stringify(postData),
                            contentType: 'application/json; charset=UTF-8',
                            success: function (_data) {
                                if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                                    if (_data.data.reservationStatus === 6) {
                                        dp.events.remove(args.source);
                                        dp.update();
                                        dp.message("Reservation is been deleteds for " + _data.data.start + " " + _data.data.end + ".");
                                    }
                                    else {
                                        dp.message("Reservation cannot been deleted for " + _data.data.start + " " + _data.data.end + ".");
                                    }
                                }
                                else {
                                    dp.message(_data.ExceptionMessage);
                                    console.error("error: ", _data.ExceptionMessage);
                                }
                            },
                            error: function (xhr, errorType, exception) {
                                dp.message("error: " + xhr + " " + errorType + " " + exception);
                                console.error("error: ", xhr, " ", errorType, " ", exception);
                            }
                        });
                    }
                },
                { text: "-" },
                {
                    text: "Select",
                    onClick: (args) => {
                        dp.multiselect.add(args.source);
                    }
                }
            ]
        }),
        onEventMoved: (args) => {
            const postData = {
                start : args.newStart.value,
                end : args.newEnd.value,
                resource : args.newResource,
                email : _email,
                reservationId : args.e.data.id
            };

            $.ajax({
                type: 'POST',
                url: _updateUrl,
                datatype: 'json',
                cache: false,
                data: JSON.stringify(postData),
                contentType: 'application/json; charset=UTF-8',
                success: function (_data) {
                    if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                        var oldEvent = scheduler.events.find(function(e) {
                            return e.id === args.e.data.id;
                        });

                        if (oldEvent) {
                            dp.events.remove(oldEvent);

                            dp.events.add({
                                start: new DayPilot.Date(_data.data.start),
                                end: new DayPilot.Date(_data.data.end),
                                id: _data.data.id,
                                resource: _data.data.resource,
                                text: _data.data.text,
                                bubbleHtml: _data.data.bubbleHtml,
                            });

                            dp.update();
                            dp.message("Reservation is been updated for " + _data.data.start + " " + _data.data.end + ".");
                        }
                        else {
                            dp.message("Reservation cannot been updated for " + _data.data.start + " " + _data.data.end + ".");
                        }
                    }
                    else {
                        dp.message(_data.ExceptionMessage);
                        console.error("error: ", _data.ExceptionMessage);
                    }
                },
                error: function (xhr, errorType, exception) {
                    dp.message("error: " + xhr + " " + errorType + " " + exception);
                    console.error("error: ", xhr, " ", errorType, " ", exception);
                }
            });
        },
        onEventMoving: (args) => {
            args.allowed = true;
        },
        onEventResized: (args) => {
            const postData = {
                start : args.newStart.value,
                end : args.newEnd.value,
                resource : args.newResource,
                email : _email,
                reservationId : args.e.data.id
            };

            $.ajax({
                type: 'POST',
                url: _updateUrl,
                datatype: 'json',
                cache: false,
                data: JSON.stringify(postData),
                contentType: 'application/json; charset=UTF-8',
                success: function (_data) {
                    if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                        var oldEvent = scheduler.events.find(function(e) {
                            return e.id === args.e.data.id;
                        });

                        if (oldEvent) {
                            dp.events.remove(oldEvent);

                            dp.events.add({
                                start: new DayPilot.Date(_data.data.start),
                                end: new DayPilot.Date(_data.data.end),
                                id: _data.data.id,
                                resource: _data.data.resource,
                                text: _data.data.text,
                                bubbleHtml: _data.data.bubbleHtml,
                            });

                            dp.update();
                            dp.message("Reservation is been updated for " + _data.data.start + " " + _data.data.end + ".");
                        }
                        else {
                            dp.message("Reservation cannot been updated for " + _data.data.start + " " + _data.data.end + ".");
                        }
                    }
                    else {
                        dp.message(_data.ExceptionMessage);
                        console.error("error: ", _data.ExceptionMessage);
                    }
                },
                error: function (xhr, errorType, exception) {
                    dp.message("error: " + xhr + " " + errorType + " " + exception);
                    console.error("error: ", xhr, " ", errorType, " ", exception);
                }
            });
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
                        dp.events.add({
                            start: new DayPilot.Date(_data.data.start),
                            end: new DayPilot.Date(_data.data.end),
                            id: _data.data.id,
                            resource: _data.data.resource,
                            text: _data.data.text,
                            bubbleHtml: _data.data.bubbleHtml,
                        });
                        dp.update();
                        dp.message("Reservation is been created for " + _data.data.start + " " + _data.data.end + ".");
                    }
                    else {
                        dp.message(_data.ExceptionMessage);
                        console.error("error: ", _data.ExceptionMessage);
                    }
                },
                error: function (xhr, errorType, exception) {
                    dp.message("error: " + xhr + " " + errorType + " " + exception);
                    console.error("error: ", xhr, " ", errorType, " ", exception);
                }
            });
        },
        onEventMove: (args) => {
            const postData = {
                start : args.newStart.value,
                end : args.newEnd.value,
                resource : args.newResource,
                email : _email,
                reservationId : args.e.data.id
            };

            $.ajax({
                type: 'POST',
                url: _updateUrl,
                datatype: 'json',
                cache: false,
                data: JSON.stringify(postData),
                contentType: 'application/json; charset=UTF-8',
                success: function (_data) {
                    if (_data.hasError == false && _data.data !== undefined && _data.data !== null) {
                        var oldEvent = scheduler.events.find(function(e) {
                            return e.id === args.e.data.id;
                        });

                        if (oldEvent) {
                            dp.events.remove(oldEvent);

                            dp.events.add({
                                start: new DayPilot.Date(_data.data.start),
                                end: new DayPilot.Date(_data.data.end),
                                id: _data.data.id,
                                resource: _data.data.resource,
                                text: _data.data.text,
                                bubbleHtml: _data.data.bubbleHtml,
                            });

                            dp.update();
                            dp.message("Reservation is been updated for " + _data.data.start + " " + _data.data.end + ".");
                        }
                        else {
                            dp.message("Reservation cannot been updated for " + _data.data.start + " " + _data.data.end + ".");
                        }
                    }
                    else {
                        dp.message(_data.ExceptionMessage);
                        console.error("error: ", _data.ExceptionMessage);
                    }
                },
                error: function (xhr, errorType, exception) {
                    dp.message("error: " + xhr + " " + errorType + " " + exception);
                    console.error("error: ", xhr, " ", errorType, " ", exception);
                }
            });
        },
        onEventClick: (args) => {
            DayPilot.Modal.alert(args.e.data.text);
        }
    });

    dp.init();
    dp.scrollTo(_startDate);

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
            const postData = {
                email : _email
            };

            $.ajax({
                type: 'POST',
                url: _listUrl,
                datatype: 'json',
                cache: false,
                data: JSON.stringify(postData),
                contentType: 'application/json; charset=UTF-8',
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
                        dp.message(_data.ExceptionMessage);
                        console.error("error: ", _data.ExceptionMessage);
                    }
                },
                error: function (xhr, errorType, exception) {
                    dp.message("error: " + xhr + " " + errorType + " " + exception);
                    console.error("error: ", xhr, " ", errorType, " ", exception);
                }
            });
        },
    };

    app.loadData();
}
