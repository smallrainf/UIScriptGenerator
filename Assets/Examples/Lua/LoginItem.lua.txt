-------------控件定义--------------
--!@#start

--!@#definestart
LoginItem = 
{
    --Image
    Img_Bg = nil;
    --Button
    Btn_Login = nil;
    --Button
    Btn_Quit = nil;

}
--!@#defineend

--!@#get/setstart
function LoginItem:get_Img_Bg_sprite()
    if self.Img_Bg ~= nil then
        return self.Img_Bg.sprite;
    end
end
function LoginItem:set_Img_Bg_sprite(sprite)
    if self.Img_Bg ~= nil then
        self.Img_Bg.sprite = sprite;
    end
end

--!@#get/setend

-------------输入监听--------------
function LoginItem:InitBtnListener()
--!@#regclickstart
    self.csharp:AddSelfClick(self.Btn_Login.gameObject, self.OnBtn_Login, self);
    self.csharp:AddSelfClick(self.Btn_Quit.gameObject, self.OnBtn_Quit, self);

--!@#regclickend
end

--!@#clickstart
function LoginItem:OnBtn_Login(go)

end
function LoginItem:OnBtn_Quit(go)

end

--!@#clickend

--!@#end

function LoginItem:InitFEventListener()

end

--!@#feventstart

--!@#feventend

------------生命周期------------------
function LoginItem:Init()
	self:InitBtnListener();
	self:OnSpawn();
end

function LoginItem:Dispose()
  	self:OnRelease();
end

function LoginItem:OnSpawn()
	self:InitFEventListener();
end

function LoginItem:OnRelease()
  
end
