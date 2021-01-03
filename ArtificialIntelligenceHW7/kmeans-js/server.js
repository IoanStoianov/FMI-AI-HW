const express = require('express')
const app = express()
const fs = require('fs');

const path = require('path');

app.set('view engine', 'ejs');

app.get('/', function(req, res) {

    let rawdata = fs.readFileSync('data.json');

    res.render("normal", { name: rawdata });
})

app.get('/unbalanced', function(req, res) {

    let rawdata = fs.readFileSync('data.json');

    res.render("unbalanced", { name: rawdata });
})

app.listen(3000)