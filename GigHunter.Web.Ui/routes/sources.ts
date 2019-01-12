import * as express from "express";
const router = express.Router();


let data: { name: string, url: string}[] = [
	{ "name": "sourceOne", "url": "www.sourceone.com" },
	{ "name": "sourceTwo", "url": "http://sourcetwo.co.uk" }
];

var sourceData = () => data;

router.get('/', function(req, res) {
	res.render('sources', {title: 'Sources', "data": sourceData() });
})

export default router;